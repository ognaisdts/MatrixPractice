using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleFollowPath : IRule {

    Queue<MovementRequest> movementRequests = new Queue<MovementRequest>();
    int rowSize = 0;
    int colSize = 0;
    Cell[,] cells = null;
    List<Element> moveElements = new List<Element>();

    public RuleFollowPath(Board _board)
    {
        board = _board;
        rowSize = board.GetRowSize();
        colSize = board.GetColSize() ;
        cells = board.GetCells();

    }
    public override void Run()
    {

        for (int i = colSize - 1; i >= 0; i--)
        {
            for (int j = rowSize - 1; j >= 0; j--)
            {
                if (cells[j, i].curElement != null)
                {
                    Element element = cells[j, i].curElement.GetComponent<Element>();
                    FollowPath(element, cells[j, i]);
                }
            }

        }

        for (int j = rowSize - 1; j >= 0; j--)
        {
            int delay = 0;
            while (cells[j, 0].data == 0)
            {
                cells[j, 0].data = Random.Range(1, 4);

                int offset = 1 + (delay++);

                GameObject newObj = MatrixGameManager.Instance.CreateElementObject();
                newObj.transform.localPosition = new Vector3(-offset, cells[j, 0].position.y);
                Element newElement = newObj.GetComponent<Element>();
                newElement.SetColor(cells[j, 0].data);
                newElement.path.Enqueue(new Vector2(offset, 0));

                if (!moveElements.Contains(newElement))
                    moveElements.Add(newElement);
                //newElement.GetComponent<Element>().Move(-1-delay, 0);
                cells[j, 0].BindElement(newElement);
                FollowPath(newElement, cells[j, 0], offset);

            }
        }

        HandleMovementRequest();

    }

    void FollowPath(Element element, Cell start, int offset = 0)
    {
        Vector2Int curCoord = start.coord;

        //Queue<Vector2> path = new Queue<Vector2>();
        Cell endCell = null;
        Vector2 accuMovement = Vector2.zero + Vector2.right * offset;
        while (true)
        {
            Cell curCell = cells[curCoord.x, curCoord.y];

            Vector2Int nextCoord = new Vector2Int((int)curCell.moveDirection.y, (int)curCell.moveDirection.x) + curCoord;

            if (nextCoord.y >= colSize || nextCoord.x >= rowSize || curCell.moveDirection == Vector2.zero)
                break;


            Cell nextCell = cells[nextCoord.x, nextCoord.y];

            if (nextCell.data != 0)
                break;


            endCell = nextCell;
            accuMovement += curCell.moveDirection;
            curCoord = nextCoord;
            element.path.Enqueue(curCell.moveDirection);
            if (!moveElements.Contains(element))
                moveElements.Add(element);

        }

        if (endCell != null)
        {
            endCell.data = start.data;
            endCell.BindElement(element);
            start.Clean();
        }


    }

    void HandleMovementRequest()
    {

        foreach (var ele in moveElements)
        {
            ele.Move();
        }
        moveElements.Clear();
    }
}
