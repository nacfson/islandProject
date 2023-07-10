using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour,IInteractable,IActionable{
    private int _shakeCount = 3;
    [SerializeField] private float _shakeTimer = 3f;
    [SerializeField] private ItemListData _itemList;
    private float _dropRadius = 1.5f;

    private bool _canDrop = true;
    private bool _canShake = true;

    [SerializeField] private MeshRenderer _meshRenderer;

    public void Interact(AgentBrain<ActionData> brain){
        Debug.Log("Interact");
        brain.ChangeState(StateType.Push);
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPushAnimationEndTrigger += ChangeShakeTrue;
    }

    public void UnInteract(){
        _shakeCount = 3;
    }

    private void DropItem() {
        Debug.Log("DropItem");

        ItemObject item = _itemList.GetRandItemObj();
        for(int i = 0; i< 3; i++) {
            ItemObject obj = PoolManager.Instance.Pop(item.name) as ItemObject;
            obj.transform.position = transform.position;

            Vector3 offset = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 3),0,Mathf.Sin(Mathf.PI * 2 * i / 3)) * _dropRadius;

            bool result = Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit,10f,1 << LayerMask.NameToLayer("GROUND"));
            if(result){
                offset.y = hit.point.y + 0.2f;
            }
            else{
                Debug.LogError("Can't Find the Ground");
                return;
            }
            obj.transform.DOJump(offset,2f,1,1f).SetEase(Ease.InCirc);
        }
        _canDrop = false;
    }

    public void Shake(AgentBrain<ActionData> brain){
        if (!_canDrop || !_canShake) return;
        Debug.Log("Shake");
        _shakeCount--;
        _shakeCount = Mathf.Clamp(_shakeCount,0,3);
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.SetTriggerPush(true);
        _canShake = false;

        if(_shakeCount <= 0){
            DropItem();
            pb.AgentAnimator.OnPushAnimationEndTrigger += ChangeToIdle;
            //brain.ChangeState(StateType.Idle);
            StopAllCoroutines();
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ShakeTreeCor());
        StartCoroutine(ShakeCor(brain));
        //pb.AgentAnimator.SetTriggerPush(false);
    }

    private IEnumerator ShakeTreeCor(){
        float timer = 0f;
        float target = 0.7f;

        float origin = _meshRenderer.material.GetFloat("Vector1_abfa6146a1744fdb95fe34e6c3c07490");
        while(timer < target){
            float value = timer / target;
            timer += Time.deltaTime;
            _meshRenderer.material.SetFloat("Vector1_abfa6146a1744fdb95fe34e6c3c07490",value);
            Debug.Log($"value: {value}");
            Debug.Log($"MatValue: {_meshRenderer.material.GetFloat("Vector1_abfa6146a1744fdb95fe34e6c3c07490}")}");
            yield return null;
        }
        _meshRenderer.material.SetFloat("Vector1_abfa6146a1744fdb95fe34e6c3c07490",origin);
    }

    private IEnumerator ShakeCor(AgentBrain<ActionData> brain){
        float timer = 0f;
        while(timer < _shakeTimer){
            timer += Time.deltaTime;
            yield return null;
        }
        brain.ChangeState(StateType.Idle);        
    }

    public void DoAction(AgentBrain<ActionData> brain) {
        Shake(brain);
    }
    public void UnAction(AgentBrain<ActionData> brain) {
        _shakeCount = 3;
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeShakeTrue;
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeToIdle;
        ChangeShakeTrue(brain);
    }

    public void ChangeShakeTrue(AgentBrain<ActionData> brain){
        _canShake = true;
    }

    private void ChangeToIdle(AgentBrain<ActionData> brain){
        brain.ChangeState(StateType.Idle);
    }
}