using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Soldier> soldierList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        soldierList = new List<Soldier>();
    }

    public override string ToString()
    {
        string soldierString = "";
        foreach (Soldier soldier in soldierList)
        {
            soldierString += soldier + "\n";
        }

        return gridPosition.ToString() + "\n" + soldierString;
    }

    public void AddSoldier( Soldier soldier)
    {
        soldierList.Add(soldier);
    }

    public void RemoveSoldier(Soldier soldier)
    {
        soldierList.Remove(soldier);
    }

    public List<Soldier> GetSoldierList()
    {
        return soldierList;
    }

    public bool HasAnySoldier()
    {
        return soldierList.Count > 0;
    }

    public Soldier GetSoldier()
    {
        if(HasAnySoldier())
        {
            return soldierList[0];
        }
        else
        {
            return null;
        }
    }

}
