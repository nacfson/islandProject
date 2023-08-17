using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ERarity
{
    COMMON = 1,
    RARE = 2,
    UNIQUE = 3
}
[CreateAssetMenu(menuName = "SO/Item/Fish")]
public class FishData : Item
{
    public ERarity rarity;
}
