using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BlockScript))]
public class BlockEditorScript : Editor {
    public bool OpenVegetation;
    SerializedProperty GrassProp;
    SerializedProperty TexProp;
    SerializedProperty DroppedProp;

    public void OnEnable()
    {
        GrassProp = serializedObject.FindProperty("Grass");
        TexProp = serializedObject.FindProperty("Tex");
        DroppedProp = serializedObject.FindProperty("DroppedObj");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        BlockScript myBlockScript = (BlockScript)target;
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Type");
            myBlockScript.Type = EditorGUILayout.TextField(myBlockScript.Type);
        EditorGUILayout.EndHorizontal();
        myBlockScript.MultiTexture = EditorGUILayout.Toggle("MultiTexture", myBlockScript.MultiTexture);
        if (myBlockScript.MultiTexture)
            EditorGUILayout.PropertyField(TexProp, new GUIContent("Tex"));
        if (myBlockScript.Type == "Dirt")
        {
            OpenVegetation = EditorGUILayout.Foldout(OpenVegetation, "Vegetation");
            if (OpenVegetation)
                EditorGUILayout.PropertyField(GrassProp,new GUIContent("Grass"));
        }
        EditorGUILayout.PropertyField(DroppedProp,new GUIContent("Dropped"));
        EditorGUILayout.LabelField("",myBlockScript.health.ToString());
        serializedObject.ApplyModifiedProperties();
    }
}
