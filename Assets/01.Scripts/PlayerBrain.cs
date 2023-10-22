using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Animations.Rigging;

public enum StateType
{
    Idle = 0, UI = 1, Push = 2, Pick = 3, Entry = 4,Tool =5,Fishing = 6
}

public class PlayerBrain : AgentBrain<ActionData>
{
    public NormalState CurrentState => _currentState;
    [SerializeField] protected NormalState _currentState;

    [SerializeField] protected Input_PlayerInput _playerInput;
    public Input_PlayerInput PlayerInput => _playerInput;

    protected NormalState _previousState;
    public NormalState PrevState
    {
        get
        {
            if(_previousState == null)
            {
                Debug.LogError($"Previous State is null");
            }
            return _previousState;
        }
    }

    private Dictionary<StateType, NormalState> _stateDictionary;
    private List<PlayerAgent> _agents;
    
    private AgentAnimator _agentAnimator;
    public AgentAnimator AgentAnimator => _agentAnimator;
    private AgentMovement _agentMovement;
    public AgentMovement AgentMovement => _agentMovement;
    
    
    private AgentTool _agentTool;
    public AgentTool AgentTool => _agentTool;

    public override void SetUp(Transform agent)
    {
        _agents = new List<PlayerAgent>();
        _stateDictionary = new Dictionary<StateType, NormalState>();

        GetComponentsInChildren(_agents);
        _agents.ForEach(a => a.SetUp(agent));

        //AgentAnimator는 PlayerAgent가 아니여서 GetCmoponent로 찾아와줌
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _agentAnimator.SetUp(this.transform);
        
        _agentMovement = _agents.Find(a => a.GetType() == typeof(AgentMovement)) as AgentMovement;
        _agentTool = _agents.Find(a => a.GetType() == typeof(AgentTool)) as AgentTool;

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

        _actionData = FindTransform("ActionData").GetComponent<ActionData>();

        _agentAnimator.OnOpenAnimationEndTrigger += (AgentBrain<ActionData> brain) => UIManager.Instance.FadeSequence(2f, () => ChangeState(StateType.Idle));
        _currentState.OnEnterState();
    }

    protected virtual void Update()
    {
        _currentState.UpdateState();
    }

    public override void ChangeState(object state)
    {
        _previousState = _currentState;
        _currentState.OnExitState();
        _currentState = _stateDictionary[(StateType)state];
        _currentState.OnEnterState();
        Debug.Log($"PreviousState: {PrevState} CurrentState: {CurrentState}");

    }
    public Transform FindTransform(string childName,Transform parentTrm = null)
    {
        Transform returnTrm;
        if(parentTrm != null)
        {
            returnTrm = parentTrm.Find(childName);
        }
        else
        {
            returnTrm = transform.Find(childName);
        }
        return returnTrm;
    }


}