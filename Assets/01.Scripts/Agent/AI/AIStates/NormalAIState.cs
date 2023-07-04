using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalAIState : MonoBehaviour, IState {
    [HideInInspector] public List<AITransition> transitions;
    protected AIBrain _brain;
    protected AIActionData _actionData;
    protected AIAnimator _agentAnimator;

    protected NavMovement _navMovement;
    protected bool _isSetUp = false;

    public abstract void OnEnterState();

    public abstract void OnExitState();

    public virtual void UpdateState() {
        if(!_isSetUp) return;
        foreach (AITransition t in transitions) {
            if (t.MakeATransition()) {
                _brain.ChangeState(t.NextState);
            }
        }
    }

    public virtual void SetUp(Transform agent) {
        _brain = agent.GetComponent<AIBrain>();
        _actionData = agent.Find("AI").GetComponent<AIActionData>();
        _navMovement = agent.GetComponent<NavMovement>();
        _agentAnimator = agent.Find("Visual").GetComponent<AIAnimator>();

        transitions = new List<AITransition>();
        GetComponentsInChildren<AITransition>(transitions);
        transitions.ForEach(t => t.SetUp(agent));
        _isSetUp = true;
    }
}