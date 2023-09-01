using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
[RequireComponent(typeof(Collider))]
public class Water : ToolHandler
{
    //낚싯대 포스
    private Vector3 _bobberPos;

    private FishDataList _fishList;

    private float _minAppearTime = 5f;
    private float _maxAppearTime = 5f;
    
    private FishData _returnFish;
    private ItemRarityData _itemRarityData;
    private Transform _playerTrm;
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

    public void Interact(IActionable actionable,Transform playerTrm,Vector3 bobberPos)
    {
        if(_fishList == null)
        {
            _fishList = GameManager.Instance.FishDataList;
        }
        
        RegisterActionable(actionable);
        this._playerTrm = playerTrm;
        this._bobberPos = bobberPos;
        Debug.Log(String.Format("BobberPos: {0}",_bobberPos));
        StartCoroutine(FishCor());
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
        float targetTime = Random.Range(_minAppearTime,_maxAppearTime);
        float showIconTime = Random.Range(1f, 1.5f);
        
        yield return new WaitForSeconds(targetTime);
        Debug.Log("Can Catch Fish");
        Vector3 spawnPos = _playerTrm.position + Vector3.up * 2f;
        EmphasizeIcon icon = PoolManager.Instance.Pop("ICON") as EmphasizeIcon;
        icon.Appear(showIconTime);
        //Emphasize Icon 실행
        //여기서 물고기를 낚아야 된다는 신호를 보내야 함
        _returnFish = _fishList.GetFishDataWithRarity(this.ItemRarityData);
        yield return new WaitForSeconds(showIconTime);
        _returnFish = null;
    }

    public FishData GetFish() => _returnFish;
}
