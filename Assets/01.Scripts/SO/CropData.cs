using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Crop")]
public class CropData : Item
{
    public Mesh[] meshs;
    public Material mat;

    public int maxLevel;
    [Tooltip("It's about minute")]
    public int time;
}
