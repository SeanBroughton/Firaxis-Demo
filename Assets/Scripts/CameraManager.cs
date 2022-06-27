using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start() 
    {
           BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
           BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

           HideActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:

            Soldier shooterSoldier = shootAction.GetSoldier();
            Soldier targetSoldier = shootAction.GetTargetSoldier();
            
            Vector3 cameraCharacterHright = Vector3.up * 1.7f;

            Vector3 shootDirection = (targetSoldier.GetWorldPosition() - shooterSoldier.GetWorldPosition()).normalized;

            float shoulderOffsetAmount = 0.5f;
            Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

            Vector3 actionCameraPosition = shooterSoldier.GetWorldPosition() + cameraCharacterHright + shoulderOffset + (shootDirection * -1);

            actionCameraGameObject.transform.position = actionCameraPosition;

            actionCameraGameObject.transform.LookAt(targetSoldier.GetWorldPosition() + cameraCharacterHright);

            ShowActionCamera();
            break;
        }
    }

     private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
            HideActionCamera();
            break;
        }
    }

}
