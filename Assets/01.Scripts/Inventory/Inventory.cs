using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory{
    public int amount;
    public Item item;

    public void AddItem(Item item, int amount){
        if(this.item != item){
            Debug.LogError("Item is not same");
            return;
        }

        this.amount += amount;
    }
}