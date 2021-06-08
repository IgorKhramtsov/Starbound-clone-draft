using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGeneration))]
public class MapGenEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGeneration myMapGeneration = (MapGeneration)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("MapInfo:");
            EditorGUILayout.LabelField("MaxHeightSurface", myMapGeneration.MaxHeightSurface.ToString());
            EditorGUILayout.LabelField("MaxLowestSurface", myMapGeneration.MaxLowestSurface.ToString());
            EditorGUILayout.LabelField("OreInfo:");
            if (myMapGeneration.CoalOreCount > 0) EditorGUILayout.LabelField("Coal:", myMapGeneration.CoalOreCount.ToString());
            if (myMapGeneration.CopperOreCount > 0) EditorGUILayout.LabelField("Copper:", myMapGeneration.CopperOreCount.ToString());
            if (myMapGeneration.IronOreCount > 0) EditorGUILayout.LabelField("Iron:", myMapGeneration.IronOreCount.ToString());
            if (myMapGeneration.TinOreCount > 0) EditorGUILayout.LabelField("Tin:", myMapGeneration.CopperOreCount.ToString());

     }
}
