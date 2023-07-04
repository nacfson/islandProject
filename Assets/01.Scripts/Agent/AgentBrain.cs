using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentBrain : MonoBehaviour, IControllable {
    public ActionData AD => _actionData;
    protected ActionData _actionData;

    public MovementData MoveData => _movementData;
    [SerializeField] protected MovementData _movementData;

    public abstract void ChangeState(object state);
    //ActionData ã�Ƽ� �־������
    public abstract void SetUp(Transform agent);

    protected virtual void Awake() {
        SetUp(this.transform);
    }
}
