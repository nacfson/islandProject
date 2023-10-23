using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentInteract : PlayerAgent
{
    [SerializeField] protected float _detectionRadius = 1f;
    public override void SetUp(Transform agent)
    {
        base.SetUp(agent);
        _newInput.OnInteractKeyPress += Interact;
    }

    private void OnDisable()
    {
        _newInput.OnInteractKeyPress -= Interact;
    }

    public void Interact()
    {
        if (!_brain.GetAD().CanInteract || _brain.GetAD().IsInteracting) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius, 1 << LayerMask.NameToLayer("INTERACTABLE"));

        float closestDistance = Mathf.Infinity;
        Collider closestCollider = null;

        // 가장 가까운 콜라이더 찾기
        foreach (Collider collider in colliders)
        {
            // 플레이어 자신의 콜라이더는 제외
            if (collider.gameObject == this.gameObject) continue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        Action<IInteractable> action = (i) =>
        {
            _brain.Interactable = i;
            _agentMovement.RotateToVector(closestCollider.transform.position);
            i.Interact(_brain);
            _brain.GetAD().IsInteracting = true;
        };
        // 가장 가까운 콜라이더 사용하기
        if (closestCollider != null)
        {
            // 여기에서 가장 가까운 콜라이더에 대한 동작을 수행할 수 있습니다.
            Debug.Log("가장 가까운 콜라이더: " + closestCollider.name);
            if (closestCollider.TryGetComponent<IInteractable>(out IInteractable i)) action(i);
            else if(closestCollider.transform.parent != null) 
                if (closestCollider.transform.parent.TryGetComponent(out IInteractable ii)) action(ii);

            if (closestCollider.TryGetComponent<IActionable>(out IActionable a)) _brain.Actionable = a;
            else if (closestCollider.transform.parent != null)
                if (closestCollider.transform.parent.TryGetComponent(out IActionable aa)) _brain.Actionable = aa;

        }
    }


    public void UnInteract()
    {
        if (_brain.Interactable != null)
        {
            _brain.Interactable.UnInteract(_brain);
            _brain.Interactable = null;
            _brain.GetAD().IsInteracting = false;
        }
    }
}