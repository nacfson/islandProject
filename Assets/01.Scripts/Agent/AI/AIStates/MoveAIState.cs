using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAIState : NormalAIState {
    public override void OnEnterState() {
        Debug.Log("OnMoveAIState");
    }

    public override void OnExitState() {
    }

    public override void UpdateState() {
        base.UpdateState();
    }
}