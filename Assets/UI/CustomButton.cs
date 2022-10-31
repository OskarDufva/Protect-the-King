using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class CustomButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridManager grid = (GridManager)target;
        if(GUILayout.Button("Generate grid"))
        {
            grid.GenerateGrid();
        }

    }


}
