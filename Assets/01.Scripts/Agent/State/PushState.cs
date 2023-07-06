using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : NormalState{
    public override void OnEnterState(){
        _brain.GetAD().IsPushing = true;
        _brain.GetAD().IsInteracting = true;
    }

    public override void OnExitState(){

    }

    public override void UpdateState(){
        if(Input.GetMouseButtonDown(0)){
            _brain.Interactable.Interact(_brain);
        }

        if(_agentMovement.GetMovementSpeed(1f) > 0f){
            _brain.ChangeState(StateType.Idle);
        }
    }
}