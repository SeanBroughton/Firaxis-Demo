using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoldierRagdollSpawner : MonoBehaviour
{
   
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake() 
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;

    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        SoldierRagdoll soldierRagdoll = ragdollTransform.GetComponent<SoldierRagdoll>();
        soldierRagdoll.Setup(originalRootBone);
    }

}
