using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour {
    public bool IsReverse;
    protected AIBrain _brain;
    public abstract bool MakeADecision();
    public virtual void SetUp(Transform agent) {
        _brain = agent.GetComponent<AIBrain>();
    }
}