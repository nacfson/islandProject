using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : NormalState{
    public override void OnEnterState(){
        _brain.GetAD().IsPushing = true;
        _brain.GetAD().IsInteracting = true;
        _agentAnimator.SetBoolPush(true);
    }

    public override void OnExitState(){
        _agentAnimator.SetBoolPush(false);
    }

    public override void UpdateState(){
        if(_agentMovement.GetMovementSpeed(1f) > 0f){
            _brain.ChangeState(StateType.Idle);
        }
    }
}