using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FishingRod : MonoBehaviour, IActionable
{
    private Transform _bobber;

    public void Awake()
    {
        _bobber = transform.Find("Bobber");
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
        Vector3 endValue = Vector3.zero;
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
