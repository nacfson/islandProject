using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalState : MonoBehaviour, IState{
    protected AgentBrain<ActionData> _brain;
    protected AgentMovement _agentMovement;
    protected AgentAnimator _agentAnimator;
    protected AgentInteract _agentInteract;
    
    public abstract void OnEnterState();

    public abstract void OnExitState();
 
    public abstract void UpdateState();

    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AgentBrain<ActionData>>();
        _agentMovement = agent.GetComponent<AgentMovement>();
        _agentInteract = agent.GetComponent<AgentInteract>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();
    }
}
