using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Soldier targetSoldier;
        public Soldier shootingSoldier;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
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
        OnAnyShoot?.Invoke(this, new OnShootEventArgs {
            targetSoldier = targetSoldier,
            shootingSoldier = soldier
        });
        
        OnShoot?.Invoke(this, new OnShootEventArgs {
            targetSoldier = targetSoldier,
            shootingSoldier = soldier
        });

        targetSoldier.Damage(40);
    }

    public override string GetActionName()
    {
        return "Fire";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition soldierGridPosition = soldier.GetGridPosition();
        return GetValidActionGridPositionList(soldierGridPosition);

    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition soldierGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

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

                Vector3 soldierWorldPosition = LevelGrid.Instance.GetWorldPosition(soldierGridPosition);
                Vector3 shootDirection = (targetSoldier.GetWorldPosition() - soldierWorldPosition).normalized;
                float soldierShoulderHeight = 1.7f;
                if (Physics.Raycast(
                    soldier.GetWorldPosition() + Vector3.up * soldierShoulderHeight,
                    shootDirection,
                    Vector3.Distance(soldierWorldPosition, targetSoldier.GetWorldPosition()),
                    obstaclesLayerMask))
                    {
                        //Blocked from firing by a obstacle
                        continue;
                    }
                    

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetSoldier = LevelGrid.Instance.GetSoldierAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingOffStateTime = 1f;
        stateTimer = aimingOffStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Soldier GetTargetSoldier()
    {
        return targetSoldier;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

       public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

       Soldier targetSoldier = LevelGrid.Instance.GetSoldierAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetSoldier.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

}
