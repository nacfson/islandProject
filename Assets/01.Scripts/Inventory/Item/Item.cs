using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item",fileName = "Item")]
public class Item : ScriptableObject{
    public string itemName;
    public int price;
    public int sellPrice;
    public Sprite itemSprite;

    public int maxCnt = 100;


}


