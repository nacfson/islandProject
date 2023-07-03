using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalState : MonoBehaviour, IState{
    protected AgentBrain _brain;
    protected AgentMovement _agentMovement;
    protected AgentAnimator _agentAnimator;
    
    public abstract void OnEnterState();

    public abstract void OnExitState();
 
    public abstract void UpdateState();

    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AgentBrain>();
        _agentMovement = agent.GetComponent<AgentMovement>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();
    }
}
