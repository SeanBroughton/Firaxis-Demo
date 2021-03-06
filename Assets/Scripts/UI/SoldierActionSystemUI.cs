using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SoldierActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    

    private void Start()
    {
        SoldierActionSystem.Instance.OnSelectedSoldierChange += SoldierActionSystem_OnSelectedSoldierChange;
        SoldierActionSystem.Instance.OnSelectedActionChange += SoldierActionSystem_OnSelectedSoldierChange;
        SoldierActionSystem.Instance.OnActionStarted += SoldierActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Soldier.OnAnyActionPointsChange += Soldier_OnAnyActionPointsChange;

        UpdateActionPoints();
        CreateSoldierActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateSoldierActionButtons()
    {

        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Soldier selectedSoldier = SoldierActionSystem.Instance.GetSelectedSoldier();

        foreach (BaseAction baseAction in selectedSoldier.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void SoldierActionSystem_OnSelectedSoldierChange(object sender, EventArgs e)
    {
        CreateSoldierActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void SoldierActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void SoldierActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Soldier selectedSoldier =  SoldierActionSystem.Instance.GetSelectedSoldier();
        
        actionPointsText.text = "Action Points: " + selectedSoldier.GetActonPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Soldier_OnAnyActionPointsChange( object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

}
