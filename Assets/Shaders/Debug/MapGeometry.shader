Shader "Debug/MapGeometry" 
{
	Properties 
	{
		_Wall ("Wall", 2D) = "white" {}
		_Floor ("Floor", 2D) = "white" {}
		_Frequency ("Frequency", float) = 1
		_XDim ("X Dim", float) = 2
		_YDim ("Y Dim", float) = 2
		_Speed ("Speed", float) = 1	
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "RenderType"="Transparent" }
		LOD 200
		Alphatest Greater 0 ZWrite On ColorMask RGB Cull back
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert approxview
		
		sampler2D _Wall;
		sampler2D _Floor;
		float _Frequency;
		float _XDim;
		float _YDim;
		float _Speed;
		
		struct Input 
		{
			float4 screenPos;
			float2 uv_Wall;
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			float4 color : COLOR;
			float4 HPos;
			float2 UVx;
		};
		
		void vert (inout appdata_full v, out Input data) 
		{
			float2 scale = 1 / float2( _XDim, _YDim );
			float index = floor(_Time.w * _Speed);
			
			data.worldPos = mul (UNITY_MATRIX_MVP, v.vertex);
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			
			data.UVx = data.UVx + float2(index,floor(index/_YDim));
			
			data.UVx *= scale;
		}
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			// NOTE: assuming no bottom-facing, otherwise use abs()
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(1/_XDim,1/_YDim);
			float2 UV = screenUV+IN.UVx.xy;
			float trans;
			
			half4 c = float4(1,1,1,1);
			c = tex2D (_Floor, UV * _Frequency);
			o.Emission = c.rgb;
			o.Alpha = c.r*(1-saturate(IN.worldPos.y));
		}
 		ENDCG
	} 
	FallBack "Diffuse"
}


