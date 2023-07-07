using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent<T> : MonoBehaviour where T : ActionData{
    protected AgentBrain<T> _brain;
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;

    protected bool _isSetUp = false;

    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AgentBrain<T>>();
        _agentInput = agent.GetComponent<AgentInput>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();

        _isSetUp = true;
    }
}
