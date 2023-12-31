using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;

public class TalkAI : AIInteractable
{
    [SerializeField] protected TalkData _talkData;

    public override void Interact(AgentBrain<ActionData> brain)
    {
        UT_MainUI.Instance.StartTalk(_talkData, gameObject.name, () => GameManager.Instance.PlayerBrain.ChangeState(StateType.Idle));
        _brain.GetAD().IsTalking = true;

        var br = _brain as AIBrain; //typeCasting
        br.NavMovement.LookRotation(brain.transform.position);
    }
    public override void UnInteract(AgentBrain<ActionData> brain)
    {
        _brain.GetAD().IsTalking = false;
    }
}