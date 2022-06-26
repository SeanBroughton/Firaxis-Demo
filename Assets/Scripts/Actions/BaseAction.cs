using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    protected Soldier soldier;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        soldier = GetComponent<Soldier>();
    }
}
