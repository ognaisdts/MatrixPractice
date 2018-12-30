using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell {
    public Element curElement;
    public Vector2 position = Vector2.zero;
    public Vector2Int coord = Vector2Int.zero;
    public int data = 0; // record color use byte to improve latter
    public Vector2 moveDirection = Vector2.right;
    public bool isVisited = false; 

    public void Init(Vector2 _pos, Vector2Int _coord, int _data)
    {
        position = _pos;
        data = _data;
        coord = _coord;
    }

    public void BindElement(Element _element)
    {
        curElement = _element;
        curElement.GetComponent<Element>().SetOwenrCell(this);
   }

    public void Clean()
    {
        data = 0;
        curElement = null;
    }
}
