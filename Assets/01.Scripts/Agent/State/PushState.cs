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
    }
}