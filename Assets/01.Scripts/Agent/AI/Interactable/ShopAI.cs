using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;
using System.Linq;

public class ShopAI : AIInteractable
{
    [SerializeField] private ItemListData _items;
    [SerializeField] private TalkData _talKData;
    public override void Interact(AgentBrain<ActionData> brain)
    {
        UT_MainUI.Instance.StartTalk(_talKData,"≥ ±ºªÛ¿Œ",null);
        UT_MainUI.Instance.ShowShopUI(_items.itemList.ToHashSet<Item>());
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