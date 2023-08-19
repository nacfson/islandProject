using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRarity
{
    public ERarity rairty;
    //나중에 가중치로 바꿔도 상관 없긴 할 듯 일단 퍼센트로
    public float percent;
}

[CreateAssetMenu(menuName = "SO/Item/Rarity")]
public class ItemRarityData : ScriptableObject
{
    public List<ItemRarity> itemRarityList = new List<ItemRarity>();
}
