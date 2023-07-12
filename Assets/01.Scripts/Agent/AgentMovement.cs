using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AgentMovement : Agent<ActionData>{
    [SerializeField] protected float _gravity = -9.81f;

    protected CharacterController _charController;
    protected Vector3 _movementVelocity;
    protected float _verticalSpeed;

    public bool IsActiveMove = false;

    public override void SetUp(Transform agent){
        base.SetUp(agent);

        //AgentInput이 Setup 되어있지 않은 상황도 생각해야함
        _charController = GetComponent<CharacterController>();
        _agentInput.OnMoementKeyPress += SetMovementVelocity;
        _agentInput.OnRunKeyPress += SetRun;
    }

    private void FixedUpdate() {
        if(!IsActiveMove) return;

        if(_movementVelocity.sqrMagnitude > 0f){
            CalculateMovement();
        }
        if(_charController.isGrounded){
            _verticalSpeed = 0f;
        }
        else{
            _verticalSpeed += _gravity * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalSpeed * Vector3.up;
        //Debug.Log($"CurrentSpeed: {_movementVelocity.sqrMagnitude * 100f}");
        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude * 100f);

        _charController.Move(move);
    }
    public void GoToVector(Vector3 dir,Action Callback = null){
        float distance = Vector3.Distance(transform.position,dir);
        StartCoroutine(GoToVectorCor(dir,distance,Callback));
    }

    //IsActiveMove가 켜져있으면 원래 위치로 순간이동 해버림
    IEnumerator GoToVectorCor(Vector3 targetPos,float dist,Action Callback){
        IsActiveMove = false;
        while(Vector3.Distance(transform.position,targetPos) >= 1f){
            transform.position += (targetPos - transform.position) * Time.fixedDeltaTime;
            yield return null;
        }
        //_charController.Move(transform.position);
        
        IsActiveMove = true;
        StopImmediately();
        Callback?.Invoke();
    }

    public void RotateToVector(Vector3 dir){
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void CalculateMovement(){
        _movementVelocity.Normalize();
        if(_brain.GetAD().IsRun){
            _movementVelocity *= _brain.MoveData.RunSpeed * Time.fixedDeltaTime;
        }
        else{
            _movementVelocity *= _brain.MoveData.Speed * Time.fixedDeltaTime;
        }
        RotateToVector(_movementVelocity);
    }

    private void SetMovementVelocity(Vector3 movement){
        _movementVelocity = movement;
        _charController.Move(Vector3.zero);
        //_charController.velocity = Vector3.zero;
    }
    
    public void StopImmediately(){
        _movementVelocity = Vector3.zero;
        //_charController.Move(transform.position);
    }

    public float GetMovementSpeed(float multiply){
        return _charController.velocity.sqrMagnitude * multiply;
    }
    public void SetRun(){
        _brain.GetAD().IsRun = !_brain.GetAD().IsRun;
    }
}