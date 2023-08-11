using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.IsActiveMove = false;
        _agentMovement.GoToVector(_brain.GetAD().TargetPos, () => _agentAnimator.SetTriggerOpen(true));
    }

    public override void OnExitState()
    {
        Debug.Log("OnExitEntryState");
    }

    public override void UpdateState()
    {
        
    }
}