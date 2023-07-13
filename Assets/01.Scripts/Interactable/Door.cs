using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable{
    [SerializeField] private Vector3 _targetPos;
    private Transform _pivot;

    private void Awake() {
        _pivot = transform.Find("DoorPivot");
    } 

    public void Interact(AgentBrain<ActionData> brain){
        brain.GetAD().TargetPos = _pivot.position;
        brain.ChangeState(StateType.Entry);
    }

    public void UnInteract(AgentBrain<ActionData> brain){
        //Debug.LogError("UnInteract");
        brain.transform.position = _targetPos;
        Debug.Log(_targetPos);
    }
}