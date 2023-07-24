using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public int sellPrice;
    public int uniqueID = 0;
    public Sprite itemSprite;

    public int maxCnt = 100;
}


