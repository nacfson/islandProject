using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction : PlayerAgent
{
    public override void SetUp(Transform agent)
    {
        base.SetUp(agent);
        _newInput.OnActionKeyPress += DoAction;
    }

    public void DoAction()
    {
        if(!_brain.GetAD().CanAction) return;
        IActionable actionable = _brain.Actionable;
        if (actionable != null)
        {
            actionable.DoAction(_brain);
        }

        //Debug.Log("DOAction");      
    }
    public void UnAction()
    {
        IActionable actionable = _brain.Actionable;
        if (actionable != null)
        {
            actionable.UnAction(_brain);
            actionable = null;
            //_brain.GetAD().IsAction = false;
        }
    }
}