using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Pos/AITarget")]
public class TargetPosData : ScriptableObject
{
    private Vector3[] _targetPos;
    [SerializeField] private string _childName;

    public void SetPosDatas()
    {
        Transform[] trms = GameManager.Instance.GetPosDatas(_childName);
        _targetPos = new Vector3[trms.Length];
        for (int i = 0; i < trms.Length; i++) _targetPos[i] = trms[i].position;
    }

    public Vector3 GetRandomTargetPos()
    {
        try
        {
            int idx = Random.Range(0, _targetPos.Length);
            return _targetPos[idx];
        }
        catch
        {
            Debug.LogError("Didn't setting _targetPos! ");
            return Vector3.zero;
        }
    }
}
