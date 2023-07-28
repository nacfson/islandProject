using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Farm : MonoBehaviour
{
    [SerializeField] private float _canPlantDistance = 1f;

    /// <summary>
    ///������ �Ÿ����� pos���� �Ÿ��� �� ������
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CanPlanCrop(Vector3 pos) => Vector3.Distance(transform.position, pos) < _canPlantDistance;
    /// <summary>
    /// �Ĺ��� ũ�⿡ ���� ���۹��� ��ġ ��ġ�� ����ؾ���
    /// </summary>
    /// <param name="pos"></param>
    public void AddCrop(Vector3 pos)
    {

    }
}
