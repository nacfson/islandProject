using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAIState : NormalAIState
{
    public override void OnEnterState()
    {
        _brain.GetAD().CanMove = true;
        _navMovement.NavMeshAgent.enabled = true;
        _navMovement.SetDestination(_brain.TargetPosData.GetRandomTargetPos());
        _agentAnimator.SetSpeed(0.3f);
    }

    public override void OnExitState()
    {
        _navMovement.StopImmediately();
        _agentAnimator.InitAllAnimations();
        _brain.GetAD().CanMove = false;
        _navMovement.NavMeshAgent.enabled = false;
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (_brain.GetAD().CanMove == false) return;

        if (_navMovement.IsArrived())
        {
            Vector3 pos = _brain.TargetPosData.GetRandomTargetPos();
            _navMovement.SetDestination(pos);
        }
    }
}