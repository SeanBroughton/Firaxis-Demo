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

        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemySoldier.GetBaseActionArray())
        {
            if(!enemySoldier.CanSpendActionPointsToTakeAction(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if(bestEnemyAIAction != null && enemySoldier.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

}
