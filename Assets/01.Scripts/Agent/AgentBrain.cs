using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum StateType{
    Idle = 0 ,UI = 1,
}

public class AgentBrain : MonoBehaviour{
    public NormalState CurrentState => _currentState;
    [SerializeField] private NormalState _currentState;

    [SerializeField] private MovementData _movementData;
    public MovementData MoveData => _movementData;

    public ActionData AD;

    private Dictionary<StateType,NormalState> _stateDictionary;

    private List<Agent> _agents;

    private void Awake() {
        SetUp(this.transform);
    }


    private void SetUp(Transform agent){
        _agents = new List<Agent>();
        _stateDictionary = new Dictionary<StateType, NormalState>();

        GetComponentsInChildren<Agent>(_agents);
        _agents.ForEach(a => a.SetUp(agent));
        
        Transform stateTrm = transform.Find("States");

        foreach(StateType state in Enum.GetValues(typeof(StateType))){
            NormalState stateScript = stateTrm.GetComponent($"{state}State") as NormalState;

            if(stateScript == null){
                Debug.LogError($"There is no Script: {state}");
                return;
            }
            stateScript.SetUp(agent);
            _stateDictionary.Add(state,stateScript);
        }

        AD = transform.Find("ActionData").GetComponent<ActionData>();
    }

    private void Update() {
        _currentState.UpdateState();
    }
    
    public void ChangeState(StateType state){
        _currentState.OnExitState();
        Debug.Log($"ExitState: {_currentState}");
        _currentState = _stateDictionary[state];
        _currentState.OnEnterState();
        Debug.Log($"EnterState: {_currentState}");
    }
}