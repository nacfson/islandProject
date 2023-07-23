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

    public event Action<AgentBrain<ActionData>> OnPushAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnPickAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnOpenAnimationStartTrigger;
    public event Action<AgentBrain<ActionData>> OnOpenAnimationEndTrigger;

    protected Animator _animator;

    public override void SetUp(Transform agent)
    {
        _animator = GetComponent<Animator>();
        base.SetUp(agent);
    }
    public void SetSpeed(float value) => _animator.SetFloat(_speedHash, value);
    public void SetBoolPush(bool value) => _animator.SetBool(_pushBoolHash, value);
    public void SetTriggerPush(bool result) => SetAnimatorTrigger(result, _pushTriggerHash);
    public void SetTriggerPick(bool result) => SetAnimatorTrigger(result, _pickTriggerHash);
    public void SetTriggerOpen(bool result) => SetAnimatorTrigger(result, _openTriggerHash);
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
}