using UnityEngine;
using System.Collections;

public class Planetoid : MonoBehaviour 
{
	public Transform coordinateSystem;
	public int mass;
	
	// Use this for initialization
	void Start() 
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	
	}
	
	void OnDrawGizmos()
	{	
		if (coordinateSystem != null)
		{
			Debug.DrawRay(coordinateSystem.position, coordinateSystem.up, Color.white, 0.5f);
		}
	}
}
