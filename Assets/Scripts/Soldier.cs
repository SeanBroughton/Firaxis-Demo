using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    
    [SerializeField] private Animator soldierAnimator;
    private Vector3 targetPosition;
    private GridPosition gridPosition;

    //keeps Soldier from overlapping
    private void Awake() 
    {
            targetPosition = transform.position;
    }

    //uses the level grid to accurately mark the position of the soldier in the level
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddSoldierAtGridPosition(gridPosition, this);
    }

    //moves the player at a certain speed and direction when pressing the T key
    private void Update() 
    {

    //stopping distance stabilizes the soldier when moving and stopping at the new location
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
    //rotates the soldier to face moving direction
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    //animates the soldier's moving
            soldierAnimator.SetBool("IsWalking", true);
        }
        else
        {
            soldierAnimator.SetBool("IsWalking", false);
        }

        
    //updates the soldier's position when they move on the grid
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            //Unit changed Grid Position
            LevelGrid.Instance.SoldierMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
        
    }

    //creates the ability to move the soldier
    public void Move( Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

}
