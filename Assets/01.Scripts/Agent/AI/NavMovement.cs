using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMovement : Agent{
    protected NavMeshAgent _navMeshAgent;

    public void SetDestination(Vector3 pos){
        _navMeshAgent.SetDestination(pos);
    }

    public void StopImmediately(){
        _navMeshAgent.velocity = Vector3.zero;
    }

    public override void SetUp(Transform agent){
        base.SetUp(agent);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _brain.MoveData.Speed;
    }
}