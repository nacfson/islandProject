using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FishingRod : MonoBehaviour, IActionable,ITool
{
    private Transform _bobber;
    private Transform _playerTrm;

    public void Init(Transform trm)
    {
        _bobber = transform.Find("Bobber");
        this._playerTrm = trm;
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        ThrowBobber();
    }

    public void UnAction(AgentBrain<ActionData> brain)
    {

    }

    public void ThrowBobber()
    {
        Vector3 endValue = _playerTrm.forward;
        float jumpPower = 2f;
        int numJumps = 1;
        float duration = 2f;
        bool snapping = false;

        Debug.Log("ThrowBobber");

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_bobber.DOJump(endValue,jumpPower,numJumps,duration,snapping));
        sequence.AppendCallback(() => Debug.Log(String.Format("Boober Pos: {0} FihsingRod Pos: {1}",_bobber.position,transform.position)));
    }

}
