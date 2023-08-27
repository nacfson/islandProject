using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalState : MonoBehaviour, IState
{
    protected AgentBrain<ActionData> _brain;
    protected AgentMovement _agentMovement;
    protected AgentAnimator _agentAnimator;
    protected AgentInteract _agentInteract;
    protected AgentAction _agentAction;

    /// <summary>
    /// PreviousState로 전환할 때 State를 전환하기 위해서 State의 StateType을 가져올 수 있게 만듦
    /// </summary>
    [field: SerializeField]
    public StateType StateType { get; set; }

    public abstract void OnEnterState();

    public abstract void OnExitState();

    public abstract void UpdateState();

    public virtual void SetUp(Transform agent)
    {
        _brain = agent.GetComponent<AgentBrain<ActionData>>();
        _agentMovement = agent.GetComponent<AgentMovement>();
        _agentInteract = agent.GetComponent<AgentInteract>();
        _agentAction = agent.GetComponent<AgentAction>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();
    }
}
