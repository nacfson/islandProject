using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : Agent<ActionData>{
    protected readonly int _speedHash = Animator.StringToHash("SPEED");
    protected readonly int _pushBoolHash = Animator.StringToHash("IS_PUSH");
    protected readonly int _pushTriggerHash = Animator.StringToHash("PUSH");
    protected readonly int _pickTriggerHash = Animator.StringToHash("PICK");

    public event Action<AgentBrain<ActionData>> OnPushAnimationEndTrigger;
    public event Action<AgentBrain<ActionData>> OnPickAnimationEndTrigger;

    protected Animator _animator;

    public override void SetUp(Transform agent){
        _animator = GetComponent<Animator>();
        base.SetUp(agent);
    }

    public void SetSpeed(float value){
        _animator.SetFloat(_speedHash,value);
    }

    public void SetBoolPush(bool value){
        _animator.SetBool(_pushBoolHash,value);
    }
    public void SetTriggerPush(bool result){
        if(result){
            _animator.SetTrigger(_pushTriggerHash);
        }
        else{
            _animator.ResetTrigger(_pushTriggerHash);
        }
    }
    public void SetTriggerPick(bool result){
        if(result){
            _animator.SetTrigger(_pickTriggerHash);
        }
        else{
            _animator.ResetTrigger(_pickTriggerHash);
        }
    }

    public void InitAllAnimations() {
        SetSpeed(0f);
        SetBoolPush(false);
        SetTriggerPush(false);
        SetTriggerPick(false);
    }
    
    public void OnPushAnimationEnd(){
        OnPushAnimationEndTrigger?.Invoke(_brain);
    }
    public void OnPickAnimationEnd(){
        OnPickAnimationEndTrigger?.Invoke(_brain);
    }
}