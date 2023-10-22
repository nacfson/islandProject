using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AgentMovement : PlayerAgent
{
    [SerializeField] protected float _gravity = -9.81f;

    protected CharacterController _charController;
    
    protected Vector3 _moveInput;
    protected Vector3 _movementVelocity;
    protected float _verticalSpeed;

    public bool IsGround => _charController.isGrounded;
    public bool IsActiveMove {get;set;}
    public bool IsMoving => _moveInput.sqrMagnitude > 0.01f;

    public override void SetUp(Transform agent)
    {
        base.SetUp(agent);
        
        //AgentInput이 Setup 되어있지 않은 상황도 생각해야함
        _charController = GetComponent<CharacterController>();
        _newInput.OnMovementKeyPress += SetMovementVelocity;
        _newInput.OnRunKeyPress += SetRun;
    }

    private void FixedUpdate()
    {
        //if (!IsActiveMove) return;
        if (IsActiveMove)
        {
            CalculateMovement();
        }
        if (IsMoving)
        {
            Vector3 rotVec = new Vector3(_movementVelocity.x, 0f, _movementVelocity.z);
            LookRotation(rotVec, true);
        }
        ApplyGravity();
        _charController.Move(_movementVelocity);
        _agentAnimator?.SetSpeed(_moveInput.sqrMagnitude * 100f);
    }
    public void GoToVector(Vector3 dir, Action Callback = null)
    {
        StartCoroutine(GoToVectorCor(dir, Callback));
    }

    //IsActiveMove가 켜져있으면 원래 위치로 순간이동 해버림
    IEnumerator GoToVectorCor(Vector3 targetPos, Action Callback)
    {
        IsActiveMove = false;

        while (Vector3.Distance(transform.position, targetPos) >= 2f)
        {
            _charController.Move((targetPos - transform.position).normalized * _brain.MoveData.entrySpeed * Time.fixedDeltaTime);
            //걸어야 되어서 고정된 값을 넣어주지만 나중에 고쳐야 함
            _agentAnimator?.SetSpeed(0.3f);

            yield return null;
        }
        StopImmediately();
        Callback?.Invoke();
    }

    public void SetPlayerPos(Vector3 pos)
    {
        _charController.enabled = false;
        transform.position = pos;
        _charController.enabled = true;
    }

    public void RotateToVector(Vector3 dir)
    {
        Vector3 targetPos = dir - transform.position;
        targetPos.y = 0;
        var rot = Quaternion.LookRotation(targetPos);
        transform.rotation = rot;
    }

    public void LookRotation(Vector3 dir, bool sLerp)
    {
        var rot = Quaternion.LookRotation(dir);
        if (sLerp)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);
        }
        else
        {
            transform.rotation = rot;
        }
    }

    private void CalculateMovement()
    {
        _moveInput.Normalize();
        if (_brain.GetAD().IsRun)
        {
            _movementVelocity = new Vector3(_moveInput.x, 0, _moveInput.y) * _brain.MoveData.RunSpeed *
                                Time.fixedDeltaTime + _verticalSpeed * Vector3.up;
        }
        else
        {
            _movementVelocity = new Vector3(_moveInput.x, 0, _moveInput.y) * _brain.MoveData.Speed *
                                Time.fixedDeltaTime + _verticalSpeed * Vector3.up;
        }

    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalSpeed < 0f)
        {
            _verticalSpeed = -1f;
        }
        else
        {
            _verticalSpeed += _gravity * Time.fixedDeltaTime;
        }
    }
    private void SetMovementVelocity(Vector2 movement)
    {
        _moveInput = movement;
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetSpeed(0f);
    }

    public float GetMovementSpeed(float multiply) => _charController.velocity.sqrMagnitude * multiply;
    public void SetRun(bool value) => _brain.GetAD().IsRun = value;

}