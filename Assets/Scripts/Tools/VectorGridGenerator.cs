#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;




public class VectorGridGenerator : MonoBehaviour
{
	internal float width = 32.0f;
	internal float height = 32.0f;
	internal float depth = 32.0f;
	public float hDensity = 10.0f;
	public float wDensity = 10.0f;
	public float dDensity = 10.0f;
	public bool Distribute = true;
	public bool ShowDensityField = true;
	public float multiplier = 1.0f;
	
	public float Width
	{
		get
		{
			return width * multiplier;
		}
	}
	
	public float Height
	{
		get
		{
			return height * multiplier;
		}
	}
	
	public float Depth
	{
		get
		{
			return depth * multiplier;
		}
	}
	
	void Start()
	{
	}
	
	[MenuItem("Tools/Fabric Tech/Add Vector Grid Region")]
	static void Init()
	{		
		GameObject VectorGrid = GameObject.Find("Vector Grid Universe");
		if (VectorGrid == null)
		{
			VectorGrid = new GameObject("Vector Grid Universe");
		}
		GameObject NewRegion = new GameObject("Vector Grid Region");
		NewRegion.transform.parent = VectorGrid.transform;
		NewRegion.AddComponent("VectorGridGenerator");
	}
	
	void Update()
	{
	}
	
	public void ClearMesh()
	{
		VectorForceGroup VectorGroup = (VectorForceGroup) GameObject.FindObjectOfType(typeof(VectorForceGroup));
		
		if (VectorGroup)
		{
			for (int i = 0; i < VectorGroup.VectorForces.Count; i++)
			{
				DestroyImmediate(VectorGroup.VectorForces[i].gameObject);
			}
			DestroyImmediate(GetComponent(typeof(VectorForceGroup)));
		}
	}
	
	public void GenerateMesh()
	{
		DestroyImmediate(GetComponent(typeof(VectorForceGroup)));
		gameObject.AddComponent(typeof(VectorForceGroup));
		VectorForceGroup VFG = GetComponent(typeof(VectorForceGroup)) as VectorForceGroup;
		Vector3[] vfPositions;
		
		Vector3 scale = transform.localScale;
		
		int nodecount = 0;
		int nodes;
		if (Distribute == false)
		{
			nodes = Mathf.FloorToInt(((Height*scale.y/hDensity)+1)*((Width*scale.x/wDensity)+1)*((Depth*scale.z/dDensity)+1));
			//Debug.Log("Objective Spacing Found " + nodes.ToString());
			vfPositions = new Vector3[nodes];
			
			for(float yloc = -(Height*scale.y/2); yloc <= (Height*scale.y/2); yloc += hDensity)
			{
				for(float xloc = -(Width*scale.x/2); xloc <= (Width*scale.x/2); xloc += wDensity)
				{
					for(float zloc = -(Depth*scale.z/2); zloc <= (Depth*scale.z/2); zloc += dDensity)
					{
						if (nodecount >= nodes)
						{
							//Debug.LogWarning("Objective Spacing Iterated more cycles than we have nodes, Node Counter at " + nodecount.ToString());
							break;
						}
						
						vfPositions[nodecount].Set(xloc/scale.x, yloc/scale.y, zloc/scale.z);
						
						nodecount += 1;
					}
					if (nodecount >= nodes)
					{
						//Debug.LogWarning("Objective Spacing Aborting Count");
						break;
					}
				}
				if (nodecount >= nodes)
				{
					//Debug.LogWarning("OObjective Spacing Count Aborted");
					break;
				}
			}
		}
		else
		{
			nodes = Mathf.FloorToInt((hDensity+1)*(wDensity+1)*(dDensity+1));
			//Debug.Log("Distributive Spacing Found " + nodes.ToString());
			vfPositions = new Vector3[nodes];
			int iterCount = 0;
			
			for(float yloc = -(Height/2); yloc <= (Height/2); yloc += Height/hDensity)
			{
				for(float xloc = -(Width/2); xloc <= (Width/2); xloc += Width/wDensity)
				{
					for(float zloc = -(Depth/2); zloc <= (Depth/2); zloc += Depth/dDensity)
					{
						iterCount += 1;
						if (nodecount >= nodes)
						{
							//Debug.LogWarning("Distributive Spacing Iterated more cycles than we have nodes, Node Counter at " + nodecount.ToString());
							break;
						}
						
						vfPositions[nodecount].Set(xloc, yloc, zloc);
						
						nodecount += 1;
					}
					if (nodecount >= nodes)
					{
						//Debug.LogWarning("Distributive Spacing Aborting Count");
						break;
					}
				}
				if (nodecount >= nodes)
				{
					//Debug.LogWarning("Distributive Spacing Count Aborted");
					break;
				}
			}
			//Debug.Log("Iterated through " + iterCount.ToString() + " nodes");
		}
		
		VFG.VecForcePositions = vfPositions;
		VFG.SetupGrid();
	}
	
	void OnDrawGizmos()
	{
		Vector3 scale = transform.localScale;
		
		Gizmos.color = Color.blue;
		Gizmos.matrix = transform.localToWorldMatrix;
		
		Gizmos.DrawWireCube(new Vector3(0,0,0),
							new Vector3(Width, Height, Depth));
		
		if (ShowDensityField)
		{
			if (Distribute == false)
			{
				
				for(float yloc = -(Height*scale.y/2); yloc <= (Height*scale.y/2); yloc += hDensity)
				{
					for(float xloc = -(Width*scale.x/2); xloc <= (Width*scale.x/2); xloc += wDensity)
					{
						for(float zloc = -(Depth*scale.z/2); zloc <= (Depth*scale.z/2); zloc += dDensity)
						{
							// X lines
							Gizmos.DrawLine(new Vector3((Width/2), yloc/scale.y, zloc/scale.z),
											new Vector3(-(Width/2), yloc/scale.y, zloc/scale.z));
							// Z lines
							Gizmos.DrawLine(new Vector3(xloc/scale.x, yloc/scale.y, (Depth/2)),
											new Vector3(xloc/scale.x, yloc/scale.y, -(Depth/2)));
							// Y lines
							Gizmos.DrawLine(new Vector3(xloc/scale.x, (Height/2), zloc/scale.z),
											new Vector3(xloc/scale.x, -(Height/2), zloc/scale.z));
						}
					}
					
				}
			}
			else
			{
				for(float yloc = -(Height/2); yloc <= (Height/2); yloc += Height/hDensity)
				{
					for(float xloc = -(Width/2); xloc <= (Width/2); xloc += Width/wDensity)
					{
						for(float zloc = -(Width/2); zloc <= (Width/2); zloc += Depth/dDensity)
						{
							// X lines
							Gizmos.DrawLine(new Vector3((Width/2), yloc, zloc), new Vector3(-(Width/2), yloc, zloc));
							
							// Z lines
							Gizmos.DrawLine(new Vector3(xloc, yloc, (Depth/2)), new Vector3(xloc, yloc, -(Depth/2)));
							
							// Y lines
							Gizmos.DrawLine(new Vector3(xloc, (Height/2), zloc), new Vector3(xloc, -(Height/2), zloc));
						}
					}
				}
			}
		}
	}
	
	void OnDestroy()
	{
//		DestroyImmediate(GetComponent(typeof(VectorForceGroup)));
	}
}

#endif