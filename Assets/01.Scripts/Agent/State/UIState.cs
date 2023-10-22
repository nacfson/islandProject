using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : NormalState
{
    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentMovement.IsActiveMove = false;
        
        _agentAnimator.InitAllAnimations();
        _brain.GetAD().CanInteract = false;
        _brain.GetAD().CanAction = false;
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
            CameraManager.Instance.TalkMode(false);

            //기존에 있던 State로 변환 시켜주어야 함
            PlayerBrain pb = _brain as PlayerBrain;
            NormalState prevState = pb.PrevState;
            StateType type = StateType.Idle;


            // 만약 전 상태가 ToolState 였다면 ToolSTate로 전환 시켜줌 
            // 코드가 너무 하드코딩임
            if(prevState != null)
            {
                type = prevState.StateType;
                if(type == StateType.Tool)
                
                {
                    type = prevState.StateType;
                }
                else
                {
                    type = StateType.Idle;
                }
            }
            pb.ChangeState(type);
        }
    }
}