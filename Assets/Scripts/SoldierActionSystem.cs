using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierActionSystem : MonoBehaviour
{
  
    public static SoldierActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedSoldierChange;
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

            if(selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
               selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
            
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    {
        isBusy = false;
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
