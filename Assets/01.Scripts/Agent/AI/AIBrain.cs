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
    protected override void SetUp(Transform agent) {
        states = new List<NormalAIState>();

        AD = agent.Find("AI").GetComponent<AIActionData>();
        agent.Find("AI").GetComponentsInChildren(states);
        states.ForEach(s => s.SetUp(agent));
    }
    protected override void Update() {
        _currentAIState.UpdateState();
    }

    public void ChangeState(NormalAIState state) {
        _currentAIState.OnExitState();
        _currentAIState = state;
        _currentAIState.OnEnterState();
    }
}