using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : Agent{
    protected readonly int _speedHash = Animator.StringToHash("SPEED");

    protected Animator _animator;

    public override void SetUp(Transform agent){
        _animator = GetComponent<Animator>();
        base.SetUp(agent);
    }

    public void SetSpeed(float value){
        _animator.SetFloat(_speedHash,value);
    }

    public void InitAllAnimations() {
        _animator.SetFloat(_speedHash,0);
    }
}