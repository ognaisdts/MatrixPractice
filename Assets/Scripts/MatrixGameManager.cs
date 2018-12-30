using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixGameManager : MonoBehaviour {

    public enum DestroyAlgo
    {
        Recursive,
        Iterative
    }

    private static Element selectedElement = null;
    private Board board = null;
    public RuleController ruleController = null;
    public BoardData boardData = null;
    public GameObject ElementPrefab = null;
    public Transform boardSpawnPosition = null;
    public DestroyAlgo desatroAlgo = DestroyAlgo.Iterative;
    [HideInInspector]
    public Camera mainCam = null;

    private static MatrixGameManager instance;
    public static MatrixGameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MatrixGameManager>();

            return instance;
        }
    }

    // Use this for initialization
    void Start () {

        mainCam = Camera.main;

        board = new Board();
        board.Init(boardData);

        ruleController = new RuleController();
        ruleController.SetRule( new RuleFollowPath(board));

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroySameNeighbor()
    {
        if (selectedElement == null)
            return;

        Element element = selectedElement;

        if (desatroAlgo == DestroyAlgo.Iterative)
        {
            FindSameNeighborIterative(element);

        }
        else if (desatroAlgo == DestroyAlgo.Recursive)
        {
            FindSameNeighborRecursive(element);
        }
        else
        {
            return;
        }


        selectedElement = null;
        RunRule();

    }

    void FindSameNeighborIterative(Element selected)
    {
        Cell curCell = selected.owner;
        Cell[,] cells = board.GetCells();
        int rowSize = cells.GetLength(0);
        int colSize = cells.GetLength(1);
        Queue<Element> queue = new Queue<Element>();
        queue.Enqueue(curCell.curElement.GetComponent<Element>());
        int data = curCell.data;

        while (queue.Count != 0)
        {
            Element curElement = queue.Dequeue();

            if (curElement == null)
                continue;

            int curRow = curElement.owner.coord.x;
            int curCol = curElement.owner.coord.y;

            if (curRow + 1 < rowSize && cells[curRow + 1, curCol].curElement!=null)
            {
                if (cells[curRow + 1, curCol].data == data)
                    queue.Enqueue(cells[curRow + 1, curCol].curElement);
            }

            if (curRow - 1 >= 0 && cells[curRow - 1, curCol].curElement != null)
            {
                if (cells[curRow - 1, curCol].data == data)
                    queue.Enqueue(cells[curRow - 1, curCol].curElement);
            }

            if (curCol +1  < colSize && cells[curRow , curCol+1].curElement != null)
            {
                if (cells[curRow , curCol+1].data == data)
                    queue.Enqueue(cells[curRow , curCol+1].curElement);
            }

            if (curCol-1 >=0  && cells[curRow , curCol-1].curElement != null)
            {
                if (cells[curRow , curCol-1].data == data)
                    queue.Enqueue(cells[curRow , curCol-1].curElement);
            }

            curElement.owner.Clean();
            Destroy(curElement.gameObject);


        }
    }

    public void FindSameNeighborRecursive(Element seletced)
    {
        List<Element> list = new List<Element>();
        Cell curCell = seletced.owner;
        FindSameNeighborHelper(curCell.data,curCell.coord.x, curCell.coord.y, list, board.GetCells());

        for (int i = list.Count - 1; i >= 0; i--)
        {
            Element e = list[i];
            e.owner.Clean();
            list.RemoveAt(i);
            Destroy(e.gameObject);
        }

        list.Clear();


    }

    void FindSameNeighborHelper(int data, int curRow, int curCol, List<Element> list, Cell[,] cells)
    {
        int rowSize = cells.GetLength(0);
        int colSize = cells.GetLength(1);

        if (curRow >= rowSize || curCol >= colSize || curRow < 0 || curCol < 0)
        {
            return;
        }

        Cell curCell = cells[curRow, curCol];
        if (curCell.curElement == null)
        {
            return;
        }
        Element curElement = curCell.curElement.GetComponent<Element>();

        if (curElement == null || list.Contains(curElement) || curCell.data != data)
            return;

        if (!list.Contains(curElement) && curCell.data == data)
        {
            list.Add(curElement);
        }

        FindSameNeighborHelper(data, curRow + 1, curCol,list,cells);
        FindSameNeighborHelper(data, curRow - 1, curCol, list, cells);
        FindSameNeighborHelper(data, curRow, curCol + 1, list, cells);
        FindSameNeighborHelper(data, curRow, curCol - 1, list, cells);


    }

    void RunRule()
    {
        ruleController.ProcessRule();
    }

    public GameObject CreateElementObject()
    {
        return Instantiate(ElementPrefab, boardSpawnPosition);
    }

    public void UpdateSelectedElement(Element element)
    {
        if (selectedElement != null)
        {
            selectedElement.transform.localScale = 4 * Vector3.one;

            selectedElement = (selectedElement == element) ? null : element;
        }
        else
        {
            selectedElement = element;
        }

        if (selectedElement != null)
        {
            selectedElement.transform.localScale = 4.5f * Vector3.one;
        }
    }
}
