using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentBrain<T> : MonoBehaviour, IControllable where T : ActionData{
    protected ActionData _actionData;

    public MovementData MoveData => _movementData;
    [SerializeField] protected MovementData _movementData;

    protected IInteractable _interactable;
    public IInteractable Interactable{
        get{
            if(_interactable == null){
                Debug.LogError("Interactable is null!");
                return null;
            }
            return _interactable;
        }
        set{
            _interactable = value;
        }
    }

    public abstract void ChangeState(object state);
    //ActionData 찾아서 넣어줘야함
    public abstract void SetUp(Transform agent);

    protected virtual void Awake() {
        SetUp(this.transform);
    }

    public virtual T GetAD(){
        T ad = _actionData as T;
        return ad;
    }
}
