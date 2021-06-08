using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ItemScript))]
public class ItemEditorScript : Editor {
    SerializedProperty IdProp;
    SerializedProperty NameProp;
    SerializedProperty DescProp;
    SerializedProperty ItemCountProp;
    SerializedProperty TypeProp;
    SerializedProperty GOProp;
    SerializedProperty IconProp;
    public void OnEnable()
    {
        IdProp = serializedObject.FindProperty("id");
        NameProp = serializedObject.FindProperty("Name");
        DescProp = serializedObject.FindProperty("Description");
        ItemCountProp = serializedObject.FindProperty("ItemCount");
        TypeProp = serializedObject.FindProperty("Type");
        GOProp = serializedObject.FindProperty("GO");
        IconProp = serializedObject.FindProperty("icon");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ItemScript myItemScript = (ItemScript)target;
        //base.OnInspectorGUI();
        EditorGUILayout.PropertyField(ItemCountProp,new GUIContent("Кол-во"));
        EditorGUILayout.PropertyField(TypeProp,new GUIContent("TypeInput"));
        if(myItemScript.Type == true)
        {
            EditorGUILayout.PropertyField(NameProp,new GUIContent("Название предмета"));
            EditorGUILayout.PropertyField(DescProp,new GUIContent("Описание"));
            if ((myItemScript.GO == null || myItemScript.icon == null) && myItemScript.ScriptIsLoad == true)
                EditorGUILayout.HelpBox("Нет подходящих аттрибутов,проверьте скрипт.", MessageType.Error);
        }
        else
            EditorGUILayout.PropertyField(IdProp,new GUIContent("ID"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("    Item Info:");
        EditorGUILayout.LabelField("    ID", myItemScript.item.Id.ToString());
        EditorGUILayout.LabelField("    Название", myItemScript.item.Name);
        EditorGUILayout.LabelField("    Описание", myItemScript.item.Description);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("    GameObj");
        EditorGUILayout.ObjectField(myItemScript.item.ItemGO, typeof(GameObject));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("    Icon");
        EditorGUILayout.ObjectField(myItemScript.item.Icon, typeof(Sprite));
        EditorGUILayout.EndHorizontal();
            
        serializedObject.ApplyModifiedProperties();

    }

}
