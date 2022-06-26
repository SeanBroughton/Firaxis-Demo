using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoldierActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start()
    {
        SoldierActionSystem.Instance.OnSelectedSoldierChange += SoldierActionSystem_OnSelectedSoldierChange;

        CreateSoldierActionButtons();
    }

    private void CreateSoldierActionButtons()
    {

        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        Soldier selectedSoldier = SoldierActionSystem.Instance.GetSelectedSoldier();

        foreach (BaseAction baseAction in selectedSoldier.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void SoldierActionSystem_OnSelectedSoldierChange(object sender, EventArgs e)
    {
        CreateSoldierActionButtons();
    }

}
