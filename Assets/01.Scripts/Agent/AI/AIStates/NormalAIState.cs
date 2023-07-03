using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalAIState : MonoBehaviour, IState {
    [HideInInspector] public List<AITransition> transitions;
    protected AIBrain _brain;
    protected AIActionData _actionData;
    public abstract void OnEnterState();

    public abstract void OnExitState();

    public virtual void UpdateState() {
        foreach (AITransition t in transitions) {
            if (t.MakeATransition()) {
                _brain.ChangeState(t.NextState);
            }
        }
    }

    public virtual void SetUp(Transform agent) {
        _brain = agent.GetComponent<AIBrain>();
        _actionData = agent.Find("AI").GetComponent<AIActionData>();

        transitions = new List<AITransition>();
        GetComponentsInChildren<AITransition>(transitions);
        transitions.ForEach(t => t.SetUp(agent));
    }
}