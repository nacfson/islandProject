using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInteract : Agent<ActionData>{

    [SerializeField] protected float _detectionRadius = 3f;

    protected IInteractable _interactable;

    public override void SetUp(Transform agent){
        base.SetUp(agent);
        _agentInput.OnInteractKeyPress += Interact;
    }

    public void Interact(){
        if(!_brain.GetAD().CanInteract || _brain.GetAD().IsInteracting) return;

       Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius,1 << LayerMask.NameToLayer("AI"));

        float closestDistance = Mathf.Infinity;
        Collider closestCollider = null;

        // 가장 가까운 콜라이더 찾기
        foreach (Collider collider in colliders){
            // 플레이어 자신의 콜라이더는 제외
            if (collider.gameObject == this.gameObject) continue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance){
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        // 가장 가까운 콜라이더 사용하기
        if (closestCollider != null){
            // 여기에서 가장 가까운 콜라이더에 대한 동작을 수행할 수 있습니다.
            Debug.Log("가장 가까운 콜라이더: " + closestCollider.name);
            if(closestCollider.TryGetComponent<IInteractable>(out IInteractable i)){
                _interactable = i;
                i.Interact(this.transform);
                _brain.GetAD().IsInteracting = true;
            }
        }
    }

    public void UnInteract(){
        if(_interactable != null){
            _interactable.UnInteract();
            _interactable = null;
            _brain.GetAD().IsInteracting = false;
        }
    }
}