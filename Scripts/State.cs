using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public BattleEnemy battleEnemy;
    public bool justEntered;
    public abstract void enter();
    public abstract void update();
}
