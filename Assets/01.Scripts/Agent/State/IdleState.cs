using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : NormalState{
    public override void OnEnterState(){
        _agentMovement.IsActiveMove = true;
        _agentInteract.UnInteract();
        _brain.GetAD().CanInteract = true;
    }

    public override void OnExitState(){
    }

    public override void UpdateState(){
    }
}