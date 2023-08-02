using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.GoToVector(_brain.GetAD().TargetPos, () => _agentAnimator.SetTriggerOpen(true));
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
    }
}