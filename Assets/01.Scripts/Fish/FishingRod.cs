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
        _bobber = transform.Find("Bobber");
        _originBobberPos = _bobber.transform.position;
        _isThrowed = false;
        this._playerTrm = trm;
        _agentAnimator = trm.Find("Visual").GetComponent<AgentAnimator>();
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = (PlayerBrain)brain;
        if (_isThrowed)
        {
            Debug.Log("UnThrow");
            _agentAnimator.OnThrowAnimationEndTrigger -= ThrowBobber;
            _agentAnimator.SetBoolThrow(false);
            _bobber.transform.position = _originBobberPos;
            _isThrowed = false;
            pb.ChangeState(StateType.Tool);
        }
        else
        {
            Debug.Log("Throw");
            _agentAnimator.OnThrowAnimationEndTrigger += ThrowBobber;
            _agentAnimator.SetTriggerThrow(true);
            _agentAnimator.SetBoolThrow(true);
            _isThrowed = true;
            pb.ChangeState(StateType.Fishing);
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
