
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(netSynchManager))]
public class netSynchManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        netSynchManager myScript = (netSynchManager)target;
        if (GUILayout.Button("Fill list"))
        {
            myScript.findNetAgent();
        }
    }
}

#endif
