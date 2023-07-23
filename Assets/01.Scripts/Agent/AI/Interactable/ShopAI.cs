using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;
public class ShopAI : AIInteractable
{
    public override void Interact(AgentBrain<ActionData> brain)
    {
        UT_MainUI.Instance.ShowShopUI();
        AIBrain aiBrain = (AIBrain)_brain;
        aiBrain.GetAD().IsInteracting = true;
        //aiBrain.NavMovement.StopImmediately();
        aiBrain.NavMovement.LookRotation(brain.transform.position);

        brain.GetAD().IsInteracting = true;
        brain.ChangeState(StateType.UI);
    }

    public override void UnInteract(AgentBrain<ActionData> brain)
    {
        UT_MainUI.Instance.UnShowAllUI();
        brain.GetAD().IsInteracting = false;
        _brain.GetAD().IsInteracting = false;
    }
}