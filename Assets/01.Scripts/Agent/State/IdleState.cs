using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentMovement.IsActiveMove = true;
        _agentInteract.UnInteract();
        _agentAction.UnAction();
        _brain.GetAD().CanInteract = true;
        _brain.GetAD().CanAction = true;
        
        
    }

    public override void OnExitState()
    {
    }

    public override void UpdateState()
    {

    }
}