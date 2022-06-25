using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    
    [SerializeField] private Soldier soldier;

    private void Start()
    {

    }

    
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridSystemVisual.Instance.HideAllGridPosition();
            GridSystemVisual.Instance.ShowGridPositionList(
                soldier.GetMoveAction().GetValidActionGridPositionList());
            
        }
    }
}
