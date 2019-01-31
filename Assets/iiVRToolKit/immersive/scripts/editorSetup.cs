
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//[CustomEditor(typeof(iiVRUnityInterface))]
[InitializeOnLoad]
public class editorSetup : Editor
{
    static editorSetup()
    {
        EditorApplication.update += OnInit;
    }

    static void OnInit()
    {
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;
        PlayerSettings.runInBackground = true;
        PlayerSettings.visibleInBackground = true;

        EditorApplication.update -= OnInit;
    }
}

#endif
