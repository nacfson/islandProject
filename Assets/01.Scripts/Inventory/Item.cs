using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item",fileName = "Item")]
public class Item : ScriptableObject{
    public string itemName;
    public int prize;
    public Sprite itemSprite;



}