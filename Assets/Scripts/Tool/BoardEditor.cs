using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BoardEditor : MonoBehaviour {

    public GameObject cellRef = null;
    public EditableCell[] cellsInEditor= null;
    private GameObject cellsHolder = null;

    public int rowSize = 3;
    public int colSize = 3;

    public bool randomDataWhenGenerator = false;



    public void CreateBoardData()
    {
        GameObject cellsHolder = transform.Find("CellsHolder").gameObject;
        if (cellsHolder == null)
            return;

        //fetch cells in editor data
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                cellsInEditor[i * rowSize + j] = cellsHolder.transform.GetChild(i * rowSize + j).GetComponent<EditableCell>();
            }
        }

        BoardData boadData = ScriptableObject.CreateInstance<BoardData>();
        AssetDatabase.CreateAsset(boadData, "Assets/boadData.asset");
        AssetDatabase.StartAssetEditing();


        boadData.rowSize = rowSize;
        boadData.colSize = colSize;
        boadData.cells = new CellData[rowSize * colSize];
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                boadData.cells[i*rowSize+ j] = new CellData();
                boadData.cells[i * rowSize + j].data = cellsInEditor[i*rowSize + j].Data;

                if (cellsInEditor[i * rowSize + j].moveDirection == MoveDir.RIGHT)
                    boadData.cells[i * rowSize + j].moveDirection = Vector2.right;
                if (cellsInEditor[i * rowSize + j].moveDirection == MoveDir.RIGHTUP)
                    boadData.cells[i * rowSize + j].moveDirection = Vector2.right + Vector2.up;
                if (cellsInEditor[i * rowSize + j].moveDirection == MoveDir.RIHTDOWN)
                    boadData.cells[i * rowSize + j].moveDirection = Vector2.right + Vector2.down;

                if (cellsInEditor[i * rowSize + j].moveDirection == MoveDir.NONE)
                {
                    boadData.cells[i * rowSize + j].moveDirection = Vector2.zero;
                }
            }
        }

        AssetDatabase.StopAssetEditing();
        EditorUtility.SetDirty(boadData);
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = boadData;
    }


    public void GenerateBoardInEditor()
    {
        if (cellsHolder == null)
        {
            cellsHolder = new GameObject("CellsHolder");
            cellsHolder.transform.SetParent(this.transform);
        }

        if (cellsInEditor != null)
        {
            cellsInEditor = null;
        }
        Init();

    }
    public void DeleteBoard()
    {
        GameObject cellsHolder = transform.Find("CellsHolder").gameObject;

        if (cellsHolder != null)
            DestroyImmediate(cellsHolder);

        if (cellsInEditor != null)
        {
            cellsInEditor = null;
        }
    }

    public void Init()
    {
        cellsInEditor = new EditableCell[rowSize * colSize];

        //init each Cell
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                GameObject newEditableCell = Instantiate(cellRef, cellsHolder.transform) as GameObject;
                cellsInEditor[i * rowSize + j] = newEditableCell.GetComponent<EditableCell>();
                cellsInEditor[i * rowSize + j].posInCells = new Vector2(i, j);

                if (randomDataWhenGenerator)
                {
                    cellsInEditor[i * rowSize + j].Data = Random.Range(1,4);
                }

                newEditableCell.name = "Cell(" + i + "," + j + ")";
                newEditableCell.transform.localPosition = new Vector3(j, i);
            }
        }
    }
}
