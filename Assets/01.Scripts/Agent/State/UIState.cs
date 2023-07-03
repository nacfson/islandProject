using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : NormalState{
    public override void OnEnterState(){
        _agentMovement.IsActiveMove = false;
        _agentAnimator.InitAllAnimations();
    }

    public override void OnExitState(){
    }

    public override void UpdateState(){

    }
}