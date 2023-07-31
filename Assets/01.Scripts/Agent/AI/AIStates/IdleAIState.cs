using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIState : NormalAIState
{
    public override void OnEnterState()
    {
        //_navMovement.StopImmediately();
        _agentAnimator.InitAllAnimations();
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}