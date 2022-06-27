using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake() 
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update() 
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
        case State.WaitingForEnemyTurn:
            break;
        case State.TakingTurn:
          timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (TryTakeEnemyAIAction(SetStateTakingTurn))
                {
                    state = State.Busy;
                }
                else
                {
                    //All enemies have used their action points, ends enemy turn
                    TurnSystem.Instance.NextTurn();
                }
            }
            break;
        case State.Busy:
            break;
        }

      
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
        
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Taking Enemy Action!");
        foreach (Soldier enemySoldier in SoldierManager.Instance.GetEnemySoldierList())
        {
            if (TryTakeEnemyAIAction(enemySoldier, onEnemyAIActionComplete))
            {
                   return true;
            }
         
        }

        return false;

    }

    private bool TryTakeEnemyAIAction(Soldier enemySoldier,  Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemySoldier.GetSpinAction();

         GridPosition actionGridPosition = enemySoldier.GetGridPosition();

            if(!spinAction.IsValidActionGridPosition(actionGridPosition))
            {
                return false;
            }
            if(!enemySoldier.TrySpendActionPointsToTakeAction(spinAction))
            {
                return false;
            }

            Debug.Log("Spin Action!");

            spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);

            return true;
                

    }

}
