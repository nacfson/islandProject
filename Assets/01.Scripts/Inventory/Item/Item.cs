using UnityEngine;
public enum ItemType
{
    Normal = 0,
    Crop = 1,
    Tool = 2,
    Fish = 3,
}
[CreateAssetMenu(menuName = "SO/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public int sellPrice;
    public int uniqueID = 0;
    public Sprite itemSprite;
    public ItemType itemType;
    
    public int maxCnt = 100;
}


