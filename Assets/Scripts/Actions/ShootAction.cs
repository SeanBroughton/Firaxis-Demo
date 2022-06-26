using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
     private State state;
     private int maxShootDistance = 7;
     private float stateTimer;
     private Soldier targetSoldier;
     private bool canShootBullet;

     private void Update()
    {
        if(!isActive)
        {
           return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
            Vector3 aimDirection = (targetSoldier.GetWorldPosition() - soldier.GetWorldPosition()).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

         if(stateTimer <= 0f)
                {
                    NextState();
                }
    }

    private void NextState()
    {
        switch (state)
        {
        case State.Aiming:
            state = State.Shooting;
            float shootingStateTime = 0.1f;
            stateTimer = shootingStateTime;
            break;
        case State.Shooting:
            state = State.Cooloff;
            float coolOffStateTime = 0.5f;
            stateTimer = coolOffStateTime;
            break;
        case State.Cooloff:
            ActionComplete();
            break;
        }
    }

    private void Shoot()
    {
        targetSoldier.Damage();
    }

    public override string GetActionName()
    {
        return "Fire";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition soldierGridPosition = soldier.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = soldierGridPosition + offsetGridPosition;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if(testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnySoldierOnGridPosition(testGridPosition))
                {
                    //Grid position is empty, no soldiers
                    continue;
                }

                Soldier targetSoldier = LevelGrid.Instance.GetSoldierAtGridPosition(testGridPosition);

                if(targetSoldier.IsEnemy() == soldier.IsEnemy())
                {
                    //Both Soldiers are on the "Same Team"
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetSoldier = LevelGrid.Instance.GetSoldierAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingOffStateTime = 1f;
        stateTimer = aimingOffStateTime;

        canShootBullet = true;

    }
}
