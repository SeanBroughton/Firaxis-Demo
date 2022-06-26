using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierActionSystem : MonoBehaviour
{
  
    public static SoldierActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedSoldierChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnActionStarted;
    [SerializeField] private Soldier selectedSoldier;
    [SerializeField] private LayerMask soldierLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    //singleton made to check for more than one SoldierActionSystem
    private void Awake() 
    {
        if(TryHandleSoldierSelection()) return;
        if(Instance != null)
        {
            Debug.LogError("There is more than one SoldierActionSystem!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start() 
    {
        SetSelectedSoldier(selectedSoldier);
    }

    private void Update() 
    {
        //when the player clicks the right mouse button, it moves the soldier
        if(Input.GetMouseButtonDown(0))
        {
            if(isBusy)
            {
                return;
            }

            if(!TurnSystem.Instance.IsPlayerTurn())
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if(TryHandleSoldierSelection())
            {
                return;
            }
            

            HandleSelectedAction();
        }

    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }
            if(!selectedSoldier.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChange?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChange?.Invoke(this, isBusy);
    }

    //selects a soldier when a player clicks on them
     private bool TryHandleSoldierSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, soldierLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<Soldier>(out Soldier soldier))
            {

                if(soldier == selectedSoldier)
                {
                    //soldier is already selected
                    return false;
                }

                if(soldier.IsEnemy())
                {
                    //clicked on a enemy soldier
                    return false;
                }

                SetSelectedSoldier(soldier);
                return true;
            }
        }

        return false;
    }

    //tells the game that you have selected a soldier
    private void SetSelectedSoldier(Soldier soldier)
    {
        selectedSoldier = soldier;

        SetSelectedAction(soldier.GetMoveAction());

        OnSelectedSoldierChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }

    //gets the soldier the player has selected and sends a message to the selection circle
    public Soldier GetSelectedSoldier()
    {
        return selectedSoldier;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

}
