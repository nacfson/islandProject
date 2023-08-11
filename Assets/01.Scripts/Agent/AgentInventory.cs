using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;
public class AgentInventory : Agent<ActionData>{
    public override void SetUp(Transform agent){
        base.SetUp(agent);
        _agentInput.OnInventoryKeyPress += OpenInv;
    }

    public void OpenInv()
    {
        bool isOpen = UT_MainUI.Instance.IsInvOpen();
        //Debug.Log(isOpen);
        UT_MainUI.Instance.OpenInv(!isOpen);
    }
}
