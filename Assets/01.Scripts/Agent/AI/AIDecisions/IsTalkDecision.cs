using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTalkDecision : AIDecision{
    public override bool MakeADecision(){
        return _brain.GetAD().IsTalking;
    }
}