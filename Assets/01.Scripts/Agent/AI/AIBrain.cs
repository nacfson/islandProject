using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;

public class AIBrain : AgentBrain<AIActionData> ,IInteractable{
    public NormalAIState CurrentAIState => _currentAIState;
    [SerializeField] protected NormalAIState _currentAIState;

    [SerializeField] protected TalkData _talkData;
    public List<NormalAIState> states;
    private List<Agent<AIActionData>> _agents;

    //Agent<AIActionData>를 상속받고 있지 않기 때문에 따로 받아줌
    protected AIAnimator _agentAnimator;
    protected NavMovement _navMovement;

    protected override void Awake() {
        SetUp(this.transform);
    }
    public override void SetUp(Transform agent) {
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
    protected virtual void Update() {
        _currentAIState.UpdateState();
    }

    public override void ChangeState(object state) {
        _currentAIState.OnExitState();
        _currentAIState = state as NormalAIState;
        _currentAIState.OnEnterState();
    }

    public void Interact(AgentBrain<ActionData> brain){
        //나중에 이름 설정하는 것도 필요할 듯
        UT_MainUI.Instance.StartTalk(_talkData,gameObject.name);
        
        GetAD().IsTalking = true;
        _navMovement.LookRotation(brain.transform.position);
    }

    public void UnInteract(AgentBrain<ActionData> brain){
        GetAD().IsTalking = false;
    }
}