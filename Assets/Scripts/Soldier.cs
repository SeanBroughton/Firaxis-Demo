using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
    }

    //uses the level grid to accurately mark the position of the soldier in the level
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddSoldierAtGridPosition(gridPosition, this);
    }

    private void Update() 
    {

    //updates the soldier's position when they move on the grid
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            //Unit changed Grid Position
            LevelGrid.Instance.SoldierMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
        
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

}
