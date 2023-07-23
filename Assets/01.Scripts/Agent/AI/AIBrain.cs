using CustomUpdateManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIBrain : AgentBrain<AIActionData>, IUpdatable
{
    public NormalAIState CurrentAIState => _currentAIState;
    [SerializeField] protected NormalAIState _currentAIState;

    [SerializeField] protected TargetPosData _targetPosSO;
    public TargetPosData TargetPosData => _targetPosSO;

    public List<NormalAIState> states;
    private List<Agent<AIActionData>> _agents;

    //Agent<AIActionData>를 상속받고 있지 않기 때문에 따로 받아줌
    protected AIAnimator _agentAnimator;
    public NavMovement NavMovement => _navMovement;
    protected NavMovement _navMovement;



    protected override void Awake()
    {
        SetUp(this.transform);
    }

    private void OnEnable() => Add(this);
private void OnDisable() => Remove(this);
    public override void SetUp(Transform agent)
    {
        _agents = new List<Agent<AIActionData>>();

        GetComponentsInChildren<Agent<AIActionData>>(_agents);
        _agents.ForEach(a => a.SetUp(agent));

        _agentAnimator = agent.Find("Visual").GetComponent<AIAnimator>();
        _agentAnimator.SetUp(agent);

        _navMovement = agent.GetComponent<NavMovement>();

        states = new List<NormalAIState>();

        _actionData = agent.Find("AI").GetComponent<AIActionData>();
        agent.Find("AI").GetComponentsInChildren(states);
        states.ForEach(s => s.SetUp(agent));
    }
    public override void ChangeState(object state)
    {
        _currentAIState.OnExitState();
        _currentAIState = state as NormalAIState;
        _currentAIState.OnEnterState();
    }

    public virtual void CustomUpdate()
    {
        _currentAIState.UpdateState();
    }

    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);

    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
}