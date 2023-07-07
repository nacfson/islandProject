using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemList")]
public class ItemListData : ScriptableObject{
    public List<ItemObject> items = new List<ItemObject>();
    public ItemObject GetRandItemObj() {
        int rand = Random.Range(0, items.Count);
        return items[rand];
    }
}

