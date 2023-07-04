using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour{
    protected AgentBrain _brain;
    protected AgentInput _agentInput;
    protected AgentAnimator _agnetAnimator;

    protected bool _isSetUp = false;

    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AgentBrain>();
        _agentInput = agent.GetComponent<AgentInput>();
        _agnetAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();

        _isSetUp = true;
    }
}
