

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

public class removeEmptyNodes : EditorWindow
{
    GameObject _toOptimize = null;

    [MenuItem("iiVR/optimizeEmptyNodes")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(removeEmptyNodes));
    }

    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        _toOptimize = (GameObject)EditorGUI.ObjectField(new Rect(3, 25, position.width - 6, 20), "GameObject to replace", _toOptimize, typeof(GameObject));

        if (GUI.Button(new Rect(3, 50, position.width - 6, 20), "Optimize"))
            if (_toOptimize)
            {
                launchRemoveEmptyNodes();
            }

    }

    void launchRemoveEmptyNodes()
    {
        cleanChildren (_toOptimize , 0);
    }

    /// <summary>
    /// recursive function fr removing empty nodes
    /// </summary>
    /// <param name="toClean"> the object we want to clean</param>
    /// <returns>true if the object is deleted, false else</returns>
    bool cleanChildren(GameObject toClean, int level)
    {
        if (toClean.transform.childCount == 0)
        {
            if (toClean.GetComponent<Renderer>() == null)
            {
                GameObject.DestroyImmediate(toClean );
                return true;
            }
        }
        
        for(int i = 0; i < toClean.transform.childCount; i ++)
        {
            if (cleanChildren(toClean.transform.GetChild(i).gameObject, level + 1))
            {
                i = i - 1;
            }
        }
        
        return false;
    }
}

#endif