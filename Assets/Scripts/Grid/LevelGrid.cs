using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance {get; private set;}
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem gridSystem;

    //created a singleton to stop duplicates of level grid
  private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }
    
    //find the position of the soldier and marks it on the grid, then clears it when they move 
    public void AddSoldierAtGridPosition(GridPosition gridPosition, Soldier soldier)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddSoldier(soldier);
    }

    public List<Soldier> GetSoldierListAtGridPosition(GridPosition gridPosition)
    {
         GridObject gridObject = gridSystem.GetGridObject(gridPosition);
         return gridObject.GetSoldierList();
    }

    public void RemoveSoldierAtGridPosition(GridPosition gridPosition, Soldier soldier)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveSoldier(soldier);
    }

    public void SoldierMovedGridPosition(Soldier soldier, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveSoldierAtGridPosition(fromGridPosition, soldier);

        AddSoldierAtGridPosition(toGridPosition, soldier);
    }

    //gets the world position of the soldier
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

}
