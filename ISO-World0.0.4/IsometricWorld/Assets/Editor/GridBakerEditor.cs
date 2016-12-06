using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GridBake))]
public class GridBakerEditor : Editor 
{
	public override void OnInspectorGUI ()
	{
//		base.OnInspectorGUI ();

		GridBake myTarget = (GridBake)target;

		myTarget.totalHorizontalTiles = EditorGUILayout.IntField("Total horizontal tiles", myTarget.totalHorizontalTiles);
		myTarget.totalVerticalTiles = EditorGUILayout.IntField("Total vertical tiles", myTarget.totalVerticalTiles);
		myTarget.bakingStep = EditorGUILayout.IntField("Baking step", myTarget.bakingStep);

		myTarget.tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile prefab", myTarget.tilePrefab, typeof(GameObject), true);

		if(GUILayout.Button("Bake tiles"))
		{
			myTarget.StartBaking();
		}

		if(GUILayout.Button("Clear tiles"))
		{
			myTarget.ClearTiles();
		}
	}




}
