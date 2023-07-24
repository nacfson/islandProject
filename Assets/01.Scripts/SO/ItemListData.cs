using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemList")]
public class ItemListData : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}