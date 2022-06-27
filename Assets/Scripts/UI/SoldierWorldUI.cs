using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SoldierWorldUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI actionPointsText;
   [SerializeField] private Soldier soldier;
   [SerializeField] private Image healthBarImage;
   [SerializeField] private HealthSystem healthSystem;


   private void Start() 
   {
        Soldier.OnAnyActionPointsChange += Soldier_OnAnyActionPointsChange;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        
        UpdateActionPointsText();
        UpdateHealthBar();
   }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = soldier.GetActonPoints().ToString();
    }

    private void Soldier_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
        
    }


}
