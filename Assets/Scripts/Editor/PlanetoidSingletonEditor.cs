using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(PlanetoidSingleton))]
public class PlanetoidSingletonEditor : Editor
{
	PlanetoidSingleton planetoidSingleton;
	
	public void OnEnable()
	{
		planetoidSingleton = (PlanetoidSingleton) target;
	}
	
	public override void OnInspectorGUI()
	{
	   	this.DrawDefaultInspector();
	 
	    if (GUILayout.Button("Query scene for Planetoids", GUILayout.Width(255)))
	    {
	       planetoidSingleton.QueryForPlanetoids();
	    }
	 
	    SceneView.RepaintAll();
	}
}

