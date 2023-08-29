using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomUpdateManager;

public class AgentInput : Agent<ActionData>, IUpdatable
{
    public event Action<Vector2> OnMoementKeyPress;

    public event Action OnJumpKeyPress;
    public event Action OnRunKeyPress;
    public event Action OnInteractKeyPress;
    public event Action OnActionKeyPress;
    public event Action OnInventoryKeyPress;
    

    public override void SetUp(Transform agent)
    {
        base.SetUp(agent);
    }

    public void CustomUpdate()
    {
        if (!_isSetUp) return;

        GetMovementInput();
        GetJumpInput();
        GetRunInput();
        GetInteractInput();
        GetActionInput();
        GetInventoryInput();
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);

    private void GetInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInventoryKeyPress?.Invoke();
        }
    }

    private void GetActionInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnActionKeyPress?.Invoke();
        }
    }

    private void GetInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnInteractKeyPress?.Invoke();
        }
    }

    private void GetRunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnRunKeyPress?.Invoke();
        }
    }

    private void GetJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpKeyPress?.Invoke();
        }
    }

    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal"), z = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, z);
        OnMoementKeyPress?.Invoke(input);
    }

    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);

    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
}