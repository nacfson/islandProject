using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDecision : AIDecision
{
    public override bool MakeADecision()
    {
        return _brain.GetAD().IsInteracting;
    }
}