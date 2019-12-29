using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public string name;
    public string description;
    public Sprite sprite;

    public Item()
    {
        this.name = "";
        this.name = "";
        this.sprite = null;
    }
    public Item(string name)
    {
        this.name = name;
    }
    public Item(string name, string description, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
    }

}
