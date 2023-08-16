using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : Agent<ActionData>
{
    protected readonly int _speedHash = Animator.StringToHash("SPEED");
    protected readonly int _pushBoolHash = Animator.StringToHash("IS_PUSH");
    protected readonly int _pushTriggerHash = Animator.StringToHash("PUSH");
    protected readonly int _pickTriggerHash = Animator.StringToHash("PICK");
    protected readonly int _openTriggerHash = Animator.StringToHash("OPEN");
    protected readonly int _throwTriggerHash = Animator.StringToHash(("THROW"));
    protected readonly int _throwBoolHash = Animator.StringToHash(("IS_THROW"));

    public event Action<AgentBrain<ActionData>> OnPushAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnPickAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnOpenAnimationStartTrigger;
    public event Action<AgentBrain<ActionData>> OnOpenAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnThrowAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> UnThrowAnimationEndTrigger;

    protected Animator _animator;
    public override void SetUp(Transform agent)
    {
        _animator = GetComponent<Animator>();
        base.SetUp(agent);
    }
    public void SetSpeed(float value) => _animator.SetFloat(_speedHash, value);
    public void SetBoolPush(bool value) => _animator.SetBool(_pushBoolHash, value);
    public void SetBoolThrow(bool value) => _animator.SetBool(_throwBoolHash, value);
    public void SetTriggerPush(bool result) => SetAnimatorTrigger(result, _pushTriggerHash);
    public void SetTriggerPick(bool result) => SetAnimatorTrigger(result, _pickTriggerHash);
    public void SetTriggerOpen(bool result) => SetAnimatorTrigger(result, _openTriggerHash);
    public void SetTriggerThrow(bool result) => SetAnimatorTrigger(result, _throwTriggerHash);
    private void SetAnimatorTrigger(bool result,int animHash)
    {
        if (result) _animator.SetTrigger(animHash);
        else _animator.ResetTrigger(animHash);
    }
    public void InitAllAnimations()
    {
        SetSpeed(0f);
        SetBoolPush(false);
        SetTriggerPush(false);
        SetTriggerPick(false);
        SetTriggerOpen(false);
        SetTriggerThrow(false);
    }

    public void OnPushAnimationEnd()
    {
        OnPushAnimationEndTrigger?.Invoke(_brain);
    }
    public void OnPickAnimationEnd()
    {
        OnPickAnimationEndTrigger?.Invoke(_brain);
    }
    public void OnOpenAnimationEnd()
    {
        OnOpenAnimationEndTrigger?.Invoke(_brain);
    }
    public void OnOpenAnimationStart()
    {
        OnOpenAnimationStartTrigger?.Invoke(_brain);
    }
    /// <summary>
    /// 낚싯대 던지는 애니메이션에 이러한 이벤트를 추가해주어야 함
    /// </summary>
    public void OnThrowAnimationEnd()
    {
        Debug.Log("OnThrowAnimationEnd");
        OnThrowAnimationEndTrigger?.Invoke(_brain);
    }

    public void UnThrowAnimationEnd()
    {
        Debug.Log("UnThrowAnimationEndTrigger");
        UnThrowAnimationEndTrigger?.Invoke(_brain);
    }
}