using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingState : NormalState
{
    public override void OnEnterState()
    {
        Debug.Log("OnFishingState");
        _agentMovement.IsActiveMove = false;
        
    }

    public override void OnExitState()
    {
        Debug.Log("ExitFishingState");

    }

    public override void UpdateState()
    {
    }
}
