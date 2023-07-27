using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ObjList")]
public class ItemObjListData : ScriptableObject
{
    public List<ItemObject> itemObjs = new List<ItemObject>();

    public ItemObject GetRandItemObj()
    {
        int rand = Random.Range(0, itemObjs.Count);
        return itemObjs[rand];
    }
}

