using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(VectorGridGenerator))]
public class VectorGridGeneratorEditor : Editor
{
	VectorGridGenerator vecGrid;
	
	public void OnEnable()
	{
		vecGrid = (VectorGridGenerator)target;
	}
	
	public override void OnInspectorGUI()
	{
	    GUILayout.BeginHorizontal();
	    GUILayout.Label("Height (Y) Grid Density");
	    vecGrid.hDensity = EditorGUILayout.FloatField(vecGrid.hDensity, GUILayout.Width(50));
	    GUILayout.EndHorizontal();
	 
	    GUILayout.BeginHorizontal();
	    GUILayout.Label("Width (X) Grid Density");
	    vecGrid.wDensity = EditorGUILayout.FloatField(vecGrid.wDensity, GUILayout.Width(50));
	    GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Depth (Z) Grid Density");
	    vecGrid.dDensity = EditorGUILayout.FloatField(vecGrid.dDensity, GUILayout.Width(50));
	    GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Treat Density Values as Probes Per Axis");
	    vecGrid.Distribute = EditorGUILayout.Toggle(vecGrid.Distribute, GUILayout.Width(50));
	    GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Show Density Field");
	    vecGrid.ShowDensityField = EditorGUILayout.Toggle(vecGrid.ShowDensityField, GUILayout.Width(50));
	    GUILayout.EndHorizontal();
	 
	    if (GUILayout.Button("Calculate Vector Field Coordinate System", GUILayout.Width(255)))
	    {
	       vecGrid.GenerateMesh();
	    }
		
		if (GUILayout.Button("Delete Vector Field Coordinate System", GUILayout.Width(255)))
	    {
	       vecGrid.ClearMesh();
	    }
	 
	    SceneView.RepaintAll();
	}
}

