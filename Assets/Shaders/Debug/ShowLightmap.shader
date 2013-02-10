Shader "Debug/ShowLightmap" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        
	_VirtualSpecOffset ("Specular Offset from Camera", Vector) = (0, 1, 4, 0)
	_VirtualSpecRange ("Specular Range", Float) = 20
	_VirtualSpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_VirtualShininess ("Shininess", Range (0.01, 1)) = 0.078125
    _VirtualSpecTiling ("Specular UV Tiling", Vector) = ( 1, 1, 0, 0)
    _SHLightingScale("LightProbe influence scale",float) = 1
}

SubShader 
{
	Tags { "RenderType"="Opaque" "Queue" = "Geometry" "LightMode"="ForwardBase" "RenderEffect"="Network"}
	LOD 100
	
	
	
	CGINCLUDE
	#include "UnityCG.cginc"
        #pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
	sampler2D _MainTex;
	float4 _MainTex_ST;
	samplerCUBE _ReflTex;
	
	#ifndef LIGHTMAP_OFF
	float4 unity_LightmapST;
	sampler2D unity_Lightmap;
	#endif

	//float _MainTexMipBias;
	half3 _VirtualSpecOffset;
	half _VirtualSpecRange;
	half3 _VirtualSpecColor;
	half _VirtualShininess;
    half2 _VirtualSpecTiling;
    
    float _SHLightingScale;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 pack0 : TEXCOORD0;
		#ifdef LIGHTMAP_ON
		float2 lmap : TEXCOORD1;
                #endif
                #ifdef LIGHTMAP_OFF
                fixed3 SHLighting: TEXCOORD1;       
		#endif
		fixed3 spec : TEXCOORD2;
                half2 specUV : TEXCOORD3;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		
		//o.texcoord = TRANSFORM_UV(0) + frac(_ScrollingSpeed * _Time.y);
		o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.specUV = o.pack0.xy * _VirtualSpecTiling.xy;
                float3 worldNormal = mul((float3x3)_Object2World, v.normal);    
		float3 viewNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
		float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
		float3 viewDir = float3(0,0,1);
		float3 viewLightPos = _VirtualSpecOffset * float3(1,1,-1);
		
		float3 dirToLight = viewPos.xyz - viewLightPos;
		
		float3 h = (viewDir + normalize(-dirToLight)) * 0.5;
		float atten = 1.0 - saturate(length(dirToLight) / _VirtualSpecRange);

		o.spec = _VirtualSpecColor * pow(saturate(dot(viewNormal, normalize(h))), _VirtualShininess * 128) * 2 * atten; //saturate //2
		
		#ifdef LIGHTMAP_ON
		o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                #ifdef LIGHTMAP_OFF
                o.SHLighting = ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;         
		#endif
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 c = 0;
            fixed3 tex = tex2D (_MainTex, i.pack0).rgb;
            fixed gloss = tex2D (_MainTex, i.specUV).a;
			fixed3 spec = i.spec.rgb * gloss;
			c.rgb = tex;
//			#if 1
			c.rgb += spec;
//			#else
			//c.rgb = c.rgb + spec - c.rgb * spec;
//			#endif
			
			#ifdef LIGHTMAP_ON
			fixed3 lm = DecodeLightmap (tex2D(unity_Lightmap, i.lmap));
			c.rgb *= lm;
			//c.rgb = lm
                        #endif
                        #ifdef LIGHTMAP_OFF
                        c.rgb *= i.SHLighting; 
                           
			#endif
			;
			return c;
		}
		ENDCG 
	}	
}
}
