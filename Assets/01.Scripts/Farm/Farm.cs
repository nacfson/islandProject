using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Farm : MonoBehaviour
{
    [SerializeField] private float _canPlantDistance = 1f;

    /// <summary>
    ///정해진 거리보다 pos와의 거리가 더 작은가
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CanPlanCrop(Vector3 pos) => Vector3.Distance(transform.position, pos) < _canPlantDistance;
    /// <summary>
    /// 식물의 크기에 따라 농작물의 설치 위치를 계산해야함
    /// </summary>
    /// <param name="pos"></param>
    public void AddCrop(Vector3 pos)
    {

    }
}
