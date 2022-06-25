using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Soldier> soldierList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
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

}
