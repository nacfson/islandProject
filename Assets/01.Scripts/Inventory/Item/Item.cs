using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item",fileName = "Item")]
public class Item : ScriptableObject{
    public string itemName;
    public int prize;
    public Sprite itemSprite;

    // public static bool operator==(Item item,Item item1){
    //     return item.itemName == item1.itemName;
    // }
    // public static bool operator!=(Item item,Item item1){
    //     return item.itemName != item1.itemName;
    // }
}

