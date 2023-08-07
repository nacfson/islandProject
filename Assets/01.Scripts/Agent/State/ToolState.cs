using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentMovement.IsActiveMove = true;
        _brain.GetAD().CanInteract = true;
    }

    public override void OnExitState()
    {
        _agentAction.UnAction();
    }

    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            _brain.ChangeState(StateType.Idle);
        }
    }
}
