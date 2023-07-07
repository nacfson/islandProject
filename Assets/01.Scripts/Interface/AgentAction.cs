using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction : Agent<ActionData> {
    public override void SetUp(Transform agent) {
        base.SetUp(agent);

        _agentInput.OnActionKeyPress += DoAction;
    }

    public void DoAction() {
        IActionable actionable = _brain.Actionable;
        if(actionable != null ) {
            actionable.DoAction(_brain);
        }

        Debug.Log("DOAction");      
    }
    public void UnAction() {
        IActionable actionable = _brain.Actionable;
        if(actionable != null) {
            actionable.UnAction();
            actionable = null;
            //_brain.GetAD().IsAction = false;
        }
    }
}   