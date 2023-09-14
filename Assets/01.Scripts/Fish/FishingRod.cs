using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FishingRod : MonoBehaviour, IActionable, ITool
{
    [SerializeField] private float _jumpPower = 2f;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _throwDistance = 3f;
    [SerializeField] private LayerMask _canInteractLayer;

    private Transform _bobber;
    private Vector3 _originBobberPos;
    private Transform _playerTrm;
    private Water _handler;

    private AgentAnimator _agentAnimator;

    private bool _isThrowed = false;

    public void Init(Transform trm)
    {
        _bobber = transform.Find("Bobber");
        _originBobberPos = _bobber.position;
        _isThrowed = false;
        this._playerTrm = trm;
        _agentAnimator = trm.Find("Visual").GetComponent<AgentAnimator>();
        _agentAnimator.OnThrowAnimationEndTrigger += ThrowBobber;
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = (PlayerBrain)brain;
        if (_isThrowed)
        {
            Debug.Log("UnThrow");
            _agentAnimator.SetBoolThrow(false);
            
            UnThrowBobber(pb);
            _isThrowed = false;
            pb.ChangeState(StateType.Tool);

            if (_handler != null)
            {
                FishData fish = _handler.GetFish();
                if (fish != null)
                {
                    InventoryManager.Instance.AddItem(fish, 1);
                    Debug.Log(String.Format("물고기를 획득하였습니다: {0}", fish));
                }
                _handler.UnRegisterActionable();
                _handler = null;
            }
        }
        else
        {
            Debug.Log("Throw");
            _agentAnimator.SetTriggerThrow(true);
            _agentAnimator.SetBoolThrow(true);
            _isThrowed = true;
            pb.ChangeState(StateType.Fishing);
        }
    }
    public void UnAction(AgentBrain<ActionData> brain)
    {
        Debug.Log("UnRegisterThrowBobber");
        _agentAnimator.OnThrowAnimationEndTrigger -= ThrowBobber;

    }
    private void ThrowBobber(AgentBrain<ActionData> brain)
    {
        Ray ray = new Ray(_playerTrm.position + _playerTrm.forward * _throwDistance,Vector3.down * 10f);
        bool result = Physics.Raycast(ray,out RaycastHit hit,_canInteractLayer);
        if (result == false) return;
        Vector3 endValue = hit.point;
            
        int numJumps = 1;
        bool snapping = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_bobber.DOJump(endValue, _jumpPower, numJumps, _duration, snapping));
        sequence.AppendCallback(() =>
        {
            Vector3 bobberPos = _bobber.transform.position;
            if (hit.collider.TryGetComponent<Water>(out Water water))
            {
                water.Interact(this,_playerTrm, bobberPos);
                _handler = water;
            }
        });
    }

    private void UnThrowBobber(AgentBrain<ActionData> brain)
    {
        Vector3 endValue = _originBobberPos;
        int numJumps = 1;
        bool snapping = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_bobber.DOJump(endValue, _jumpPower, numJumps, _duration, snapping));
    }

}
