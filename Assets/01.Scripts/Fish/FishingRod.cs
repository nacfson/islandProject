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
        _originBobberPos = _bobber.transform.position;
        _isThrowed = false;
        this._playerTrm = trm;
        _agentAnimator = trm.Find("Visual").GetComponent<AgentAnimator>();
        _agentAnimator.OnThrowAnimationEndTrigger += ThrowBobber;
    }

    public void EmphasizeIcon(bool result)
    {
        _playerTrm.Find("ICON").gameObject.SetActive(result);
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = (PlayerBrain)brain;
        if (_isThrowed)
        {
            Debug.Log("UnThrow");
            _agentAnimator.SetBoolThrow(false);
            _bobber.transform.position = _originBobberPos;
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
        //ThrowBobber();
    }
    public void UnAction(AgentBrain<ActionData> brain)
    {
        Debug.Log("UnRegisterThrowBobber");
        _agentAnimator.OnThrowAnimationEndTrigger -= ThrowBobber;

    }
    public void ThrowBobber(AgentBrain<ActionData> brain)
    {
        Vector3 endValue = _playerTrm.position + _playerTrm.forward * _throwDistance;
        int numJumps = 1;
        bool snapping = false;

        Debug.Log("ThrowBobber");

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_bobber.DOJump(endValue, _jumpPower, numJumps, _duration, snapping));
        sequence.AppendCallback(() =>
        {
            Vector3 bobberPos = _bobber.transform.position;
            Collider[] cols = Physics.OverlapSphere(bobberPos, 1f, _canInteractLayer);
            if (cols.Length > 0)
            {
                if (cols[0].TryGetComponent<Water>(out Water water))
                {
                    water.Interact(this, bobberPos);
                    _handler = water;
                }
            }
        });
    }

}
