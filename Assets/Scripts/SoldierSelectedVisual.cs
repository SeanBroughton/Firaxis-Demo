using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSelectedVisual : MonoBehaviour
{
   

    [SerializeField] private Soldier soldier;

    private MeshRenderer meshRenderer;

    //gets the selected circle visual
    private void Awake() 
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    //makes sure that a soldier is selected at the start of the game
    private void Start() 
    {
        SoldierActionSystem.Instance.OnSelectedSoldierChange += SoldierActionSystem_OnSelectedSoldierChange;

        UpdateVisual();
    }

    //when the player clicks on a different soldier, it updates the selection visual
    private void SoldierActionSystem_OnSelectedSoldierChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    //turns the selection visual on and off when choosing different soldiers
    private void UpdateVisual()
    {
          if(SoldierActionSystem.Instance.GetSelectedSoldier() == soldier)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy() 
    {
        SoldierActionSystem.Instance.OnSelectedSoldierChange -= SoldierActionSystem_OnSelectedSoldierChange;
    }

}
