using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoldierManager : MonoBehaviour
{
    private List<Soldier> soldierList;
    private List<Soldier> friendlySoldierList;
    private List<Soldier> enemySoldierList;

    public static SoldierManager Instance {get; private set;}

    private void Awake() 
    {
           if(Instance != null)
        {
            Debug.LogError("There is more than one SoldierManager!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        soldierList = new List<Soldier>();
        friendlySoldierList = new List<Soldier>();
        enemySoldierList = new List<Soldier>();
    }

    private void Start()
    {
        Soldier.OnAnySoldierSpawned += Soldier_OnAnySoldierSpawned;
        Soldier.OnAnySoldierDead += Soldier_OnAnySoldierDead;
    }

    private void Soldier_OnAnySoldierSpawned(object sender, EventArgs e)
    {
        Soldier soldier = sender as Soldier;

        soldierList.Add(soldier);

        if (soldier.IsEnemy())
        {
            enemySoldierList.Add(soldier);
        }
        else
        {
            friendlySoldierList.Add(soldier);
        }
    }

     private void Soldier_OnAnySoldierDead(object sender, EventArgs e)
    {
        Soldier soldier = sender as Soldier;
        
        soldierList.Remove(soldier);

        if (soldier.IsEnemy())
        {
            enemySoldierList.Remove(soldier);
        }
        else
        {
            friendlySoldierList.Remove(soldier);
        }
    }

    public List<Soldier> GetSoldierList()
    {
        return soldierList;
    }

     public List<Soldier> GetFriendlySoldierList()
    {
        return friendlySoldierList;
    }

     public List<Soldier> GetEnemySoldierList()
    {
        return enemySoldierList;
    }


}
