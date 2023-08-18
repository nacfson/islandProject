using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public class Water : ToolHandler
{
    private Vector3 _bobberPos;

    [SerializeField] private FishDataList _fishList;
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
        FishData fish = _fishList.GetFishDataWithRarity();
        while(true)
        {
                


            yield return null;
        }
    }
    
}
