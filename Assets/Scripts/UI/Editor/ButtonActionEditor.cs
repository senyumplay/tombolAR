#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ButtonAction))]
public class ButtonActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ButtonAction buttonAction = (ButtonAction)target;

        // Checkbox: IsLoadScene
        buttonAction.isLoadScene = EditorGUILayout.Toggle("Is Load Scene", buttonAction.isLoadScene);

        if (buttonAction.isLoadScene)
        {
            // Dropdown untuk scene
            string[] sceneNames = GetSceneNames();
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(sceneNames, buttonAction.sceneToLoad));
            int selectedIndex = EditorGUILayout.Popup("Scene To Load", currentIndex, sceneNames);

            if (sceneNames.Length > 0 && selectedIndex >= 0)
            {
                buttonAction.sceneToLoad = sceneNames[selectedIndex];
            }
        }
        else
        {
            // UnityEvent
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onClickCustomAction"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private string[] GetSceneNames()
    {
        int sceneCount = EditorSceneManager.sceneCountInBuildSettings;
        string[] sceneNames = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            sceneNames[i] = name;
        }

        return sceneNames;
    }
}
#endif
