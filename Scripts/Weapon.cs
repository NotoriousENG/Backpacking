using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public List<Attack> attacks;

    public Weapon()
    {
        this.name = "";
        this.description = "";
    }
    public Weapon(string name, List<Attack> attacks)
    {
        this.name = name;
        this.attacks = attacks;
    }
    public Weapon(string name, string description, Sprite sprite, List<Attack> attacks)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.attacks = attacks;
    }

}
