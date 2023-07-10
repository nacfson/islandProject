using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickState : NormalState{
    public override void OnEnterState(){
        _brain.GetAD().IsPicking = true;
        _agentMovement.IsActiveMove = false;
        _agentAnimator.SetTriggerPick(true);
    }

    public override void OnExitState(){
        _brain.GetAD().IsPicking = false;
        _agentMovement.IsActiveMove = true;
    }

    public override void UpdateState(){
    }
}
