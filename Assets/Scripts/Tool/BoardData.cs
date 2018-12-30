using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardData : ScriptableObject {
    public int rowSize;
    public int colSize;
    public CellData[] cells;
}
