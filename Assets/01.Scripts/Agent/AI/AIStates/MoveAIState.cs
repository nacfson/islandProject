using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAIState : NormalAIState
{
    public override void OnEnterState()
    {
        _navMovement.SetDestination(_brain.TargetPosData.GetRandomTargetPos());
        _agentAnimator.SetSpeed(0.3f);
    }

    public override void OnExitState()
    {
        _agentAnimator.InitAllAnimations();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        _agentAnimator.SetSpeed(_navMovement.GetSpeed(0.3f));

        if (_navMovement.IsArrived())
        {
            //Vector3 pos = GameManager.Instance.RandomTargetPos();
            Vector3 pos = _brain.TargetPosData.GetRandomTargetPos();
            _navMovement.SetDestination(pos);
        }
    }
}