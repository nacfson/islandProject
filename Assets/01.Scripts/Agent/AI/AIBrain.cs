using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : AgentBrain {
    public NormalAIState CurrentAIState => _currentAIState;
    [SerializeField] protected NormalAIState _currentAIState;

    public List<NormalAIState> states;

    protected override void Awake() {
        SetUp(this.transform);
    }
    public override void SetUp(Transform agent) {
        states = new List<NormalAIState>();

        _actionData = agent.Find("AI").GetComponent<AIActionData>();
        agent.Find("AI").GetComponentsInChildren(states);
        states.ForEach(s => s.SetUp(agent));
    }
    protected virtual void Update() {
        _currentAIState.UpdateState();
    }

    public override void ChangeState(object state) {
        _currentAIState.OnExitState();
        _currentAIState = state as NormalAIState;
        _currentAIState.OnEnterState();
    }
}