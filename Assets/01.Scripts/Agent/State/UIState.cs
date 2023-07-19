using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : NormalState{
    public override void OnEnterState(){
        _agentMovement.IsActiveMove = false;
        _agentAnimator.InitAllAnimations();
        _brain.GetAD().CanInteract = false;
    }

    public override void OnExitState(){

    }

    public override void UpdateState(){

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UI_Toolkit.UT_MainUI.Instance.UnShowAllUI();

            //여기서 NullReference 오류 발생
            _brain.ChangeState(StateType.Idle);
        }
    }
}