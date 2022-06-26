using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoldierActionSystem : MonoBehaviour
{
  
    public static SoldierActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedSoldierChange;
    [SerializeField] private Soldier selectedSoldier;
    [SerializeField] private LayerMask soldierLayerMask;

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

    private void Update() 
    {
        //when the player clicks the right mouse button, it moves the soldier
        if(Input.GetMouseButtonDown(0))
        {
            if(isBusy)
            {
                return;
            }

            if(TryHandleSoldierSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(selectedSoldier.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedSoldier.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
        }    

        if(Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedSoldier.GetSpinAction().Spin(ClearBusy);
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
                selectedSoldier = soldier;
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

        OnSelectedSoldierChange?.Invoke(this, EventArgs.Empty);
    }

    //gets the soldier the player has selected and sends a message to the selection circle
    public Soldier GetSelectedSoldier()
    {
        return selectedSoldier;
    }

}
