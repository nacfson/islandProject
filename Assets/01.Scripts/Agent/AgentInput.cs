using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentInput : Agent<ActionData>{
    public event Action<Vector3> OnMoementKeyPress;
    public event Action OnJumpKeyPress;
    public event Action OnRunKeyPress;
    public event Action OnInteractKeyPress;

    public override void SetUp(Transform agent){
        base.SetUp(agent);
    }

    private void Update(){
        if(!_isSetUp) return;

        GetMovementInput();
        GetJumpInput();
        GetRunInput();
        GetInteractInput();
    }

    private void GetInteractInput(){
        if(Input.GetKeyDown(KeyCode.F)){
            OnInteractKeyPress?.Invoke();
        }
    }

    private void GetRunInput(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            OnRunKeyPress?.Invoke();
        }
    }

    private void GetJumpInput(){
        if(Input.GetKeyDown(KeyCode.Space)){
            OnJumpKeyPress?.Invoke();
        }
    }

    private void GetMovementInput(){
        float x = Input.GetAxisRaw("Horizontal"), z = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(x,0,z);
        OnMoementKeyPress?.Invoke(input);
    }


}