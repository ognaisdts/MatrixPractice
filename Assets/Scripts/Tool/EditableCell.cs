using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MoveDir
{
    NONE,
    RIGHT,
    RIGHTUP,
    RIHTDOWN 
}


[ExecuteInEditMode]
public class EditableCell : MonoBehaviour {

    public int Data = 1; // record color use byte to improve latter
    private int preData = 0;
    public MoveDir moveDirection = MoveDir.RIGHT;
    private MoveDir prevMoveDirection = MoveDir.RIGHT;
    private Transform arrow = null;
    [HideInInspector]
    public Vector2 posInCells = Vector2.zero;
    public Sprite noneSprite = null;
    public Sprite arrowSprite = null;

    private void Awake()
    {
        preData = Data;
        prevMoveDirection = moveDirection;
        arrow = this.transform;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (moveDirection == MoveDir.RIGHT)
            arrow.rotation = Quaternion.Euler(0,0,90);
        if (moveDirection == MoveDir.RIGHTUP)
            arrow.rotation = Quaternion.Euler(0, 0, 135);
        if (moveDirection == MoveDir.RIHTDOWN)
            arrow.rotation = Quaternion.Euler(0, 0, 45);


        if (moveDirection == MoveDir.NONE)
        {
            sr.sprite = noneSprite;
        }
        else
        {
            sr.sprite = arrowSprite;
        }


        if (Data == 1)
            sr.color = Color.red;
        if (Data == 2)
            sr.color = Color.green;
        if (Data == 3)
            sr.color = Color.blue;

    }

    private void Update()
    {
        if (moveDirection != prevMoveDirection || preData != Data)
        {
            preData = Data;
            prevMoveDirection = moveDirection;
            UpdateSprite();

        }
    }


}
