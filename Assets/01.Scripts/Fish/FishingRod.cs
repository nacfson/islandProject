using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FishingRod : MonoBehaviour, IActionable,ITool
{
    private Transform _bobber;
    private Vector3 _originBobberPos;
    private Transform _playerTrm;
    private AgentAnimator _agentAnimator;
    private bool _isThrowed = false;
    public void Init(Transform trm)
    {
        _originBobberPos = _bobber.transform.position;
        _isThrowed = false;
        _bobber = transform.Find("Bobber");
        this._playerTrm = trm;
        _agentAnimator = trm.Find("Visual").GetComponent<AgentAnimator>();
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        if (_isThrowed)
        {
            _agentAnimator.OnThrowAnimationEndTrigger -= ThrowBobber;
            _bobber.transform.position = _originBobberPos;
            _isThrowed = false;
        }
        else
        {
            _agentAnimator.OnThrowAnimationEndTrigger += ThrowBobber;
            _agentAnimator.SetTriggerThrow(true);
            _isThrowed = true;
        }
        //ThrowBobber();
    }

    public void UnAction(AgentBrain<ActionData> brain)
    {
        _agentAnimator.OnThrowAnimationEndTrigger -= ThrowBobber;
    }

    public void ThrowBobber(AgentBrain<ActionData> brain)
    {
        Vector3 endValue = _playerTrm.position + _playerTrm.forward;
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
