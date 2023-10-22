using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent<T> : MonoBehaviour where T : ActionData
{
    protected AgentBrain<T> _brain;
    protected AgentAnimator _agentAnimator;
    protected AgentMovement _agentMovement;

    protected bool _isSetUp = false;

    public virtual void SetUp(Transform agent)
    {
        _brain = agent.GetComponent<AgentBrain<T>>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();
        _agentMovement = agent.GetComponent<AgentMovement>();

        _isSetUp = true;
    }
}
