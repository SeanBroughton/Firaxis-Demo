using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
   
    [SerializeField] private Animator soldierAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    //keeps Soldier from overlapping
    protected override void Awake() 
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update() 
    {
        if(!isActive)
        {
            return;
        }
        
    //stopping distance stabilizes the soldier when moving and stopping at the new location
        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

    //animates the soldier's moving
            soldierAnimator.SetBool("IsWalking", true);
        }
        else
        {
            soldierAnimator.SetBool("IsWalking", false);
           
            ActionComplete();

        }

         //rotates the soldier to face moving direction
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    }

    //creates the ability to move the soldier
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition soldierGridPosition = soldier.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = soldierGridPosition + offsetGridPosition;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(soldierGridPosition == testGridPosition)
                {
                    //A Soldier is standing on this grid
                    continue;
                }

                if (LevelGrid.Instance.HasAnySoldierOnGridPosition(testGridPosition))
                {
                    //Grid position is already occupied with another soldier
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
