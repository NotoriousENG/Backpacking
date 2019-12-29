using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
    private List<Item> backpackList;
    private Weapon equippedWeapon;

    public Backpack()
    {
        backpackList = new List<Item>();
    }

    public void addItem(Item item)
    {
        backpackList.Add(item);
    }
    public bool hasItem(string s)
    {
        foreach(Item item in backpackList)
        {
            if (item.name == s)
                return true;
        }
        return false;

    }
    public void removeItem(Item item)
    {
        int index = 0;
        for(int i = 0;i < backpackList.Count;i++)
        {
            if (backpackList[i].name == item.name)
                index = i;

        }

        backpackList.RemoveAt(index);

    }

    public Item getFirstItem()
    {
        Item item = new Item();
        if (backpackList.Count > 0)
        {
            item = backpackList[0];
        }
        return item;
    }

    public bool isEmpty()
    {
        if (backpackList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Item[] getAllItems()
    {
        Item[] items = new Item[backpackList.Count];
        for (int i = 0; i < backpackList.Count; i++)
        {
            items[i] = backpackList[i];
        }
        return items;
    }

    public Weapon getEquippedWeapon()
    {
        return equippedWeapon;
    }

    public void setEquippedWeapon(Weapon weapon)
    {
        this.equippedWeapon = weapon;
    }
}