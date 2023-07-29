using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Farm : MonoBehaviour
{
    [SerializeField] private float _canPlantDistance = 1f;
    private Collider _collider;
    private Vector3[] _vecs = new Vector3[4];

    private List<Crop> _cropList = new List<Crop>();
    public List<Crop> CropList => _cropList;

    public void SetUp()
    {
        _collider = GetComponent<Collider>();
        
    }
    public bool CanPlantCrop(Vector3 pos) => Vector3.Distance(transform.position,pos) < _canPlantDistance;

    /// <summary>
    /// 식물의 크기에 따라 농작물의 설치 위치를 계산해야함
    /// </summary>
    /// <param name="pos"></param>
    public void AddCrop(Vector3 pos,int itemID)
    {
        float x = _collider.bounds.extents.x;
        float y = _collider.bounds.extents.y;
        float z = _collider.bounds.extents.z;

        _vecs[0] = new Vector3(-x, y, -z); //left, down
        _vecs[1] = new Vector3(x, y, -z); //right, down
        _vecs[2] = new Vector3(-x, y, z); //left, up
        _vecs[3] = new Vector3(x, y, z); //right, up

        Vector3 closestVec = _vecs[0];
        float closestDistance = 100f;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _vecs.Length; i++)
        {
            float distance = Vector3.Distance(transform.position + _vecs[i], pos);
            if (distance <= closestDistance)
            {
                sb.Remove(0, sb.Length);
                closestVec = _vecs[i];
                closestDistance = distance;
                sb.Append(string.Format("ClosestVec: {0}",closestVec));
                sb.Append(string.Format("ClosestDistance: {0}\n",closestDistance));
                sb.Append(string.Format("Vec Index{0}",i));
            }
        }

        Debug.Log(sb.ToString());

        CropData cropData = (CropData)InventoryManager.Instance.GetItemFromID(itemID);

        Crop crop = (Crop)PoolManager.Instance.Pop(cropData.itemName);
        crop.transform.SetParent(GameManager.Instance.transform);
        crop.transform.position = closestVec;
    }
}
