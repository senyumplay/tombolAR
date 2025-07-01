using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventButtonSO))]
public class EventButtonSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EventButtonSO script = (EventButtonSO)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetButton"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useParameter"));

        if (script.useParameter)
        {
            EditorGUILayout.LabelField("Events With Parameter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stringGameEvent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("intGameEvent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floatGameEvent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("boolGameEvent"));

            EditorGUILayout.LabelField("Parameter Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stringValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("intValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floatValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("boolValue"));
        }
        else
        {
            EditorGUILayout.LabelField("Event Without Parameter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gameEvent"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
