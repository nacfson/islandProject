using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int amount;
    public Item item;

    public Inventory(Item item, int amount)
    {
        this.amount = amount;
        this.item = item;
    }

    public void AddItem(int amount)
    {
        if (item == null)
        {
            Debug.LogError("Can't Find Item!");
            return;
        }
        this.amount += amount;
    }
}
