using UnityEngine;
using System.Collections.Generic;
 
public class VectorForceGroup : MonoBehaviour 
{
	public Vector3[] VecForcePositions;
	public List<VectorForce> VectorForces = new List<VectorForce>();
	
	
	void Awake()
	{
		
	}
	
	public void SetupGrid()
	{
		if (VecForcePositions != null)
		{
			for (int i = 0; i < VecForcePositions.Length; i++)
			{
				GameObject force = new GameObject();
				force.AddComponent("VectorForce");
				force.name = "Vector Force " + i;
				force.transform.parent = this.transform;
				force.transform.position = VecForcePositions[i];
				VectorForces.Add((VectorForce) force.GetComponent(typeof(VectorForce)));
			}
		}
	}
}