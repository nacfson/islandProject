using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable{
    [SerializeField] private Vector3 _targetPos;

    public void Interact(AgentBrain<ActionData> brain){
        brain.GetAD().TargetPos = transform.position;
        brain.ChangeState(StateType.Entry);
    }

    public void UnInteract(AgentBrain<ActionData> brain){
        brain.transform.position = _targetPos;
    }
}