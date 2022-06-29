using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
  
    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
  
    private void Update() 
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float moveSpeed = 15f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if(Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Soldier>(out Soldier targetSoldier))
                {
                    targetSoldier.Damage(30);
                }
            }

            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }

    }


   public void Setup(GridPosition targetGridPositon, Action onGrenadeBehaviourComplete)
   {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPositon);
   }
}
