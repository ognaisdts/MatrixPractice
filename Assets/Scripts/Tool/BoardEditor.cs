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
    public BoardData dataToOpen = null;


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
                cellsInEditor[i * colSize + j] = cellsHolder.transform.GetChild(i * colSize + j).GetComponent<EditableCell>();
            }
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/BoardDatas/BoardData.asset");
        BoardData boadData = ScriptableObject.CreateInstance<BoardData>();
        AssetDatabase.CreateAsset(boadData, assetPathAndName);
        AssetDatabase.StartAssetEditing();


        boadData.rowSize = rowSize;
        boadData.colSize = colSize;
        boadData.cells = new CellData[rowSize * colSize];
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                boadData.cells[i * colSize + j] = new CellData();
                boadData.cells[i * colSize + j].data = cellsInEditor[i* colSize + j].Data;

                if (cellsInEditor[i * colSize + j].moveDirection == MoveDir.RIGHT)
                    boadData.cells[i * colSize + j].moveDirection = Vector2.right;
                if (cellsInEditor[i * colSize + j].moveDirection == MoveDir.RIGHTUP)
                    boadData.cells[i * colSize + j].moveDirection = Vector2.right + Vector2.up;
                if (cellsInEditor[i * colSize + j].moveDirection == MoveDir.RIHTDOWN)
                    boadData.cells[i * colSize + j].moveDirection = Vector2.right + Vector2.down;

                if (cellsInEditor[i * colSize + j].moveDirection == MoveDir.NONE)
                {
                    boadData.cells[i * colSize + j].moveDirection = Vector2.zero;
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
        DeleteBoard();

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

        Transform cellsHolder = transform.Find("CellsHolder");

        if (cellsHolder != null)
            DestroyImmediate(cellsHolder.gameObject);

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
                cellsInEditor[i * colSize + j] = newEditableCell.GetComponent<EditableCell>();
                cellsInEditor[i * colSize + j].posInCells = new Vector2(i, j);

                if (randomDataWhenGenerator)
                {
                    cellsInEditor[i * colSize + j].Data = Random.Range(1,4);
                }

                newEditableCell.name = "Cell(" + i + "," + j + ")";
                newEditableCell.transform.localPosition = new Vector3(j, i);
            }
        }
    }

    public void InitFromBoardData(BoardData data)
    {
        DeleteBoard();

        if (cellsHolder == null)
        {
            cellsHolder = new GameObject("CellsHolder");
            cellsHolder.transform.SetParent(this.transform);
        }

        if (cellsInEditor != null)
        {
            cellsInEditor = null;
        }

        rowSize = data.rowSize;
        colSize = data.colSize;
        cellsInEditor = new EditableCell[rowSize * colSize];

        //init each Cell
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                GameObject newEditableCell = Instantiate(cellRef, cellsHolder.transform) as GameObject;
                cellsInEditor[i * colSize + j] = newEditableCell.GetComponent<EditableCell>();
                cellsInEditor[i * colSize + j].posInCells = new Vector2(i, j);

                cellsInEditor[i * colSize + j].Data = data.cells[i * colSize + j].data;
                if (data.cells[i * colSize + j].moveDirection == Vector2.right)
                {
                    cellsInEditor[i * colSize + j].moveDirection = MoveDir.RIGHT;
                }
                else if (data.cells[i * colSize + j].moveDirection == Vector2.right + Vector2.up)
                {
                    cellsInEditor[i * colSize + j].moveDirection = MoveDir.RIGHTUP;
                }
                else if (data.cells[i * colSize + j].moveDirection == Vector2.right + Vector2.down)
                {
                    cellsInEditor[i * colSize + j].moveDirection = MoveDir.RIHTDOWN;
                }
                else
                {
                    cellsInEditor[i * colSize + j].moveDirection = MoveDir.NONE;
                }
                newEditableCell.name = "Cell(" + i + "," + j + ")";
                newEditableCell.transform.localPosition = new Vector3(j, i);
            }
        }
    }
}
