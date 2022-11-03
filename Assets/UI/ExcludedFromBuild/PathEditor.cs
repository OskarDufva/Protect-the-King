#if (UNITY_EDITOR) 

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathFinding))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PathFinding path = (PathFinding)target;
        GUILayout.Space(10);
        if (GUILayout.Button("Find path"))
        {
            path.VeryFun();
        }

    }


}
#endif
