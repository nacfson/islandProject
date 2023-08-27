using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.IsActiveMove = false;
        _agentAnimator.InitAllAnimations();
        _brain.GetAD().CanInteract = false;
        //_brain.GetAD().destroyCancellationToken
    }

    public override void OnExitState()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI_Toolkit.UT_MainUI.Instance.UnShowAllUI();
            GameManager.Instance.CamController.TalkMode(false);

            //기존에 있던 State로 변환 시켜주어야 함
            PlayerBrain pb = _brain as PlayerBrain;
            NormalState prevState = pb.PrevState;
            StateType type;
            if(prevState == null)
            {
                type = StateType.Idle;
            }
            else
            {
                type = prevState.StateType;
            }
            pb.ChangeState(type);
        }
    }
}