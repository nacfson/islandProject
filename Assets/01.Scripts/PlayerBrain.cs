using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum StateType
{
    Idle = 0, UI = 1, Push = 2, Pick = 3, Entry = 4,
}

public class PlayerBrain : AgentBrain<ActionData>
{
    public NormalState CurrentState => _currentState;
    [SerializeField] protected NormalState _currentState;

    private Dictionary<StateType, NormalState> _stateDictionary;
    private List<Agent<ActionData>> _agents;

    private AgentAnimator _agentAnimator;
    public AgentAnimator AgentAnimator => _agentAnimator;

    private AgentMovement _agentMovement;
    public AgentMovement AgentMovement => _agentMovement;

    public override void SetUp(Transform agent)
    {
        _agents = new List<Agent<ActionData>>();
        _stateDictionary = new Dictionary<StateType, NormalState>();

        GetComponentsInChildren<Agent<ActionData>>(_agents);
        _agents.ForEach(a => a.SetUp(agent));

        _agentAnimator = _agents.Find(a => a.GetType() == typeof(AgentAnimator)) as AgentAnimator;
        _agentMovement = _agents.Find(a => a.GetType() == typeof(AgentMovement)) as AgentMovement;

        Transform stateTrm = transform.Find("States");

        foreach (StateType state in Enum.GetValues(typeof(StateType)))
        {
            NormalState stateScript = stateTrm.GetComponent($"{state}State") as NormalState;

            if (stateScript == null)
            {
                Debug.LogError($"There is no Script: {state}");
                return;
            }
            stateScript.SetUp(agent);
            _stateDictionary.Add(state, stateScript);
        }

        _actionData = transform.Find("ActionData").GetComponent<ActionData>();



        _agentAnimator.OnOpenAnimationEndTrigger += (AgentBrain<ActionData> brain) => UIManager.Instance.FadeSequence(2f, () => ChangeState(StateType.Idle));
    }

    protected virtual void Update()
    {
        _currentState.UpdateState();
    }

    public override void ChangeState(object state)
    {
        _currentState.OnExitState();
        _currentState = _stateDictionary[(StateType)state];
        _currentState.OnEnterState();

    }
}