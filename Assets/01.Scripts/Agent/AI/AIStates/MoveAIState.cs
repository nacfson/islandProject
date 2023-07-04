using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAIState : NormalAIState {
    public override void OnEnterState() {
        _navMovement.SetDestination(GameManager.Instance.RandomTargetPos());
    }

    public override void OnExitState() {
    }

    public override void UpdateState() {
        base.UpdateState();
        _agentAnimator.SetSpeed(_navMovement.GetSpeed(0.3f));

        if(_navMovement.IsArrived()){
            Vector3 pos = GameManager.Instance.RandomTargetPos();
            _navMovement.SetDestination(pos);
        }
    }
}