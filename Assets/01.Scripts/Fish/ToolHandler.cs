using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class ToolHandler : MonoBehaviour
{
    protected IActionable _iActionable;
    public virtual void RegisterActionable(IActionable actionable)
    {
        _iActionable = actionable;
    }
    public virtual void UnRegisterActionable()
    {
        Debug.Log(String.Format("등록 해제된 Actionable: {0}",_iActionable));
        _iActionable = null;
    }
}
