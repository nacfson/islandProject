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

        Debug.Log(string.Format("ToolObjChildCOunt: {0}",_toolTrm.childCount));
        for(int i = 0;  i < _toolTrm.childCount; i++)
        {
            GameObject toolObj = _toolTrm.GetChild(i).gameObject;
            Debug.Log(string.Format("ToolObjectName: {0}",toolObj.name));
            if (toolObj.TryGetComponent<ITool>(out ITool tool))
            {
                tool.Init(this.transform);
            }
            toolObj.SetActive(false);
        }

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