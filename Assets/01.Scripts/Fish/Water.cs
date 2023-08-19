using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public class Water : ToolHandler
{
    private Vector3 _bobberPos;

    [SerializeField] private FishDataList _fishList;
    private ItemRarityData _itemRarityData;
    public ItemRarityData ItemRarityData
    {
        get
        {
            if(_itemRarityData == null)
            {
                _itemRarityData = GameManager.Instance.ItemRarityData;
            }
            return _itemRarityData;
        }
    }

    public void Interact(IActionable actionable,Vector3 bobberPos)
    {
        RegisterActionable(actionable);
        this._bobberPos = bobberPos;
        Debug.Log(String.Format("BobberPos: {0}",_bobberPos));
    }


    //UnInteract를 실행 시켜주어야 함
    public void UnInteract()
    {
        UnRegisterActionable();
        this._bobberPos = Vector3.zero;
        StopAllCoroutines();
    }

    private IEnumerator FishCor()
    {
        FishData fish = _fishList.GetFishDataWithRarity(this.ItemRarityData);
        while(true)
        {
            //특정상황이 되면 물고기를 소환해줌

            yield return null;
        }
    }
    
}
