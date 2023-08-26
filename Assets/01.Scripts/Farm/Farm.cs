using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public struct FarmPos
{
    public Vector3 pos;
    public bool used;
}

[RequireComponent(typeof(Collider))]
public class Farm : MonoBehaviour
{
    [SerializeField] private float _canPlantDistance = 1f;
    private Collider _collider;
    private FarmPos[] _farmPosArray = new FarmPos[4];

    public List<Crop> cropList = new List<Crop>();
    public void SetUp()
    {
        _collider = GetComponent<Collider>();
        GetComponentsInChildren(cropList);
    }
    public bool CanPlantCrop(Vector3 pos)
    {
        float distance = Vector3.Distance(transform.position, pos);
        Debug.Log(distance);
        foreach (var f in _farmPosArray)
        {
            bool result = f.used;
            if (!result)
            {
                return distance < _canPlantDistance;
            }
        }
        return false;
    }

    /// <summary>
    /// 식물의 크기에 따라 농작물의 설치 위치를 계산해야함
    /// </summary>
    /// <param name="pos"></param>
    public Crop AddCrop(Vector3 pos,int itemID)
    {
        float x = _collider.bounds.extents.x;
        float y = _collider.bounds.extents.y;
        float z = _collider.bounds.extents.z;

        _farmPosArray[0].pos = new Vector3(-x, y, -z); //left, down
        _farmPosArray[1].pos = new Vector3(x, y, -z); //right, down
        _farmPosArray[2].pos = new Vector3(-x, y, z); //left, up
        _farmPosArray[3].pos = new Vector3(x, y, z); //right, up

        Vector3 closestVec = _farmPosArray[0].pos;
        float closestDistance = 100f;
        int index = 0;
        for (int i = 0; i < _farmPosArray.Length; i++)
        {
            float distance = Vector3.Distance(transform.position + _farmPosArray[i].pos, pos);
            if (distance <= closestDistance)
            {
                if (_farmPosArray[i].used) continue;
                closestVec = _farmPosArray[i].pos;
                closestDistance = distance;
                index = i;

            }
        }

        CropData cropData = (CropData)InventoryManager.Instance.GetItemFromID(itemID);

        Crop crop = (Crop)PoolManager.Instance.Pop(cropData.itemName);
        crop.transform.SetParent(this.transform);
        crop.transform.position = Vector3.Lerp(transform.position, closestVec + transform.position,0.5f);
        cropList.Add(crop);
        _farmPosArray[index].used = true;

        return crop;
    }
}
