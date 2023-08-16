using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public class Water : ToolHandler
{
    private Vector3 _bobberPos;

    public void Interact(IActionable actionable,Vector3 bobberPos)
    {
        RegisterActionable(actionable);
        this._bobberPos = bobberPos;
        Debug.Log(String.Format("BobberPos: {0}",_bobberPos));
    }
}
