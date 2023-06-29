using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : NormalState{
    public override void OnEnterState(){
        _agentMovement.IsActiveMove = true;
    }

    public override void OnExitState(){
    }

    public override void UpdateState(){
        if(Input.GetKeyDown(KeyCode.T)){
            _brain.ChangeState(StateType.UI);
        }
    }
}