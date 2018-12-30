using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardEditor))]
public class BoardEdiotrInspector : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoardEditor myScript = (BoardEditor)target;
        if (GUILayout.Button("Generate Board"))
        {
            myScript.GenerateBoardInEditor();
        }

        if (GUILayout.Button("Delete Board"))
        {
            myScript.DeleteBoard();
        }

        if (GUILayout.Button("Create Board Data"))
        {
            myScript.CreateBoardData();
        }
    }
}
