using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Animations.Rigging;

public enum StateType
{
    Idle = 0, UI = 1, Push = 2, Pick = 3, Entry = 4,Tool =5,
}

public class PlayerBrain : AgentBrain<ActionData>
{
    public NormalState CurrentState => _currentState;
    [SerializeField] protected NormalState _currentState;

    [SerializeField] private Transform _toolTrm;
    public Transform ToolTrm => _toolTrm;

    private Dictionary<StateType, NormalState> _stateDictionary;
    private List<Agent<ActionData>> _agents;

    private TwoBoneIKConstraint _twoBone;
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

        _twoBone = FindTransform(string.Format("Visual/RigLayer/RightHandIK")).GetComponent<TwoBoneIKConstraint>();
        Debug.Log(_twoBone);
        _actionData = FindTransform("ActionData").GetComponent<ActionData>();



        _agentAnimator.OnOpenAnimationEndTrigger += (AgentBrain<ActionData> brain) => UIManager.Instance.FadeSequence(2f, () => ChangeState(StateType.Idle));

        
        for(int i = 0;  i < _toolTrm.childCount; i++)
        {
            if (_toolTrm.TryGetComponent<ITool>(out ITool tool))
            {
                tool.Init(this.transform);
            }
            _toolTrm.GetChild(i).gameObject.SetActive(false);
        }

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

    public void SetToolActive(string name, bool active)
    {
        if (active)
        {
            _twoBone.data.target = _toolTrm;
        }
        else
        {
            _twoBone.data.target = null;
        }
        _toolTrm.Find(String.Format("{0}",name)).gameObject.SetActive(active);
    }
}