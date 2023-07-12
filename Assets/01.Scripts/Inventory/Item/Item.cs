using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item",fileName = "Item")]
public class Item : ScriptableObject{
    public string itemName;
    public int prize;
    public Sprite itemSprite;

    public int maxCnt = 100;


}


