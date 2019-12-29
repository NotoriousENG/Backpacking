using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public string name;
    public int strength;
    public float accuracy;

    public Attack()
    {
        this.name = "";
        this.strength = 0;
        this.accuracy = 0.0f;
    }
    public Attack(string name, int strength, float accuracy)
    {
        this.name = name;
        this.strength = strength;
        this.accuracy = accuracy;
    }

}
