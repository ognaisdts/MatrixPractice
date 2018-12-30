using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {
    private SpriteRenderer sr = null;
    private CircleCollider2D col = null;
    private bool isSelected = false;
    public Cell owner = null;
    bool isMoving = false;
    public Queue<Vector2> path = new Queue<Vector2>();
	// Use this for initialization
	void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<CircleCollider2D>();

    }
	
	// Update is called once per frame
	void Update () {

        Interact();
	}

    public void Move(float start, float end)
    {
        //StartCoroutine(MoveHelper(start, end));

    }
    public void Move()
    {
        StartCoroutine(MoveHelper());

    }

    float unitTime = 0.2f;
    private IEnumerator MoveHelper()
    {
        isMoving = true;
        while (path.Count != 0)
        {
            Vector2 movementDelta = path.Dequeue();
            float total = movementDelta.magnitude * unitTime;
            float accuTime = 0;
            Vector2 startPos = this.transform.position;
            Vector2 endPos = startPos + movementDelta;
            while (accuTime < total)
            {
                accuTime += Time.deltaTime;
                transform.position = Vector2.Lerp(startPos, endPos, accuTime / total);
                yield return null;

            }
        }


        isMoving = false;

    }

    void Interact()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (col.OverlapPoint(wp))
            {
                MatrixGameManager.Instance.UpdateSelectedElement(this);
            }
        }
    }

    public void SetColor(int _data)
    {
        if(sr==null)
         sr = gameObject.GetComponent<SpriteRenderer>();

        if (_data == 1)
            sr.color = Color.red;
        if (_data == 2)
            sr.color = Color.green;
        if (_data == 3)
            sr.color = Color.blue;
    }

    public void SetOwenrCell(Cell _cell)
    {
        owner = _cell;
    }

    private void OnDestroy()
    {

    }

    public void PerformMovingWithPath()
    {
        while (path.Count != 0 && !isMoving)
        {
            Vector2 curPath = path.Dequeue();
            Move(curPath.x, curPath.y);
        }

    }
}


public struct MovementRequest
{
    public Vector2 delta;
    public Element elemet;
    public MovementRequest(Vector2 _delta, Element _element)
    {
        delta = _delta;
        elemet = _element;
    }
}