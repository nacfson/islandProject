using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pos/DataList")]
public class TargetPosListData : ScriptableObject
{
    public List<TargetPosData> posDatas = new List<TargetPosData>();
}
