using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board {

    public Vector2 OriPos = Vector2.zero;
    public float offset = 0;
    private int rowSize = 3;
    private int colSize = 3;
    Cell[,] cells = null;

    
    public void Init(BoardData boardData)
    {
        rowSize = boardData.rowSize;
        colSize = boardData.colSize;
        cells = new Cell[rowSize, colSize];

        //init each Cell
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                if (cells[i, j] == null)
                    cells[i, j] = new Cell();

                Cell curCell = cells[i, j];

                curCell.Init(new Vector2(j + offset, i + offset), new Vector2Int(i,j), boardData.cells[i*rowSize + j].data);
                curCell.moveDirection = boardData.cells[i * rowSize + j].moveDirection;
                //create game object on current cell
                GameObject newObj = MatrixGameManager.Instance.CreateElementObject();
                Element element = newObj.GetComponent<Element>();
                newObj.name = "Cell(" + i + "," + j + ")";
                newObj.transform.localPosition = new Vector3(curCell.position.x, curCell.position.y);
                element.SetColor(curCell.data);
                curCell.BindElement(element);
            }
        }
    }

    public int GetRowSize()
    {
        return rowSize;
    }

    public int GetColSize()
    {
        return colSize;
    }

    public Cell[,] GetCells()
    {
        return cells;
    }

}
