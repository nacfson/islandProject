using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMovement : Agent<AIActionData>{
    protected NavMeshAgent _navMeshAgent;

    public override void SetUp(Transform agent){
        base.SetUp(agent);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _brain.MoveData.Speed;
    }

    public void SetDestination(Vector3 pos){
        _navMeshAgent.SetDestination(pos);
    }
    
    //Lerp를 이용해 좀 더 자연스러운 화면 돌림을 구현하는 것도 고려
    public void LookRotation(Vector3 dir){
        var to = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Lerp(transform.rotation,to,0.1f);
        transform.rotation = to;
        
    }

    public void StopImmediately(){
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.SetDestination(transform.position);
    }


    public float GetSpeed(float multiplyValue){
        return _navMeshAgent.velocity.sqrMagnitude * multiplyValue;
    }

    public bool IsArrived(){
        if(_navMeshAgent.pathPending) return false;

        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance){
            return true;
        }
        return false;
    }
}