using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour,IInteractable,IActionable{
    private int _shakeCount = 3;
    [SerializeField] private float _shakeTimer = 3f;
    [SerializeField] private ItemListData _itemList;

    private bool _canDrop = true;
    private bool _canShake = true;


    public void Interact(AgentBrain<ActionData> brain){
        Debug.Log("Interact");
        brain.ChangeState(StateType.Push);
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPushAnimationEndTrigger += ChangeShakeFalse;

        //audience.transform.Find("Visual").GetComponent<AgentAnimator>().SetBoolPush(true);
    }

    public void UnInteract(){
        _shakeCount = 3;
    }

    private void DropItem() {
        Debug.Log("DropItem");

        ItemObject item = _itemList.GetRandItemObj();
        for(int i = 0; i< 3; i++) {
            ItemObject obj = Instantiate(item);
            obj.transform.position = transform.position;
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
        StartCoroutine(ShakeCor(brain));
        //pb.AgentAnimator.SetTriggerPush(false);
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
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeShakeFalse;
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeToIdle;
    }

    public void ChangeShakeFalse(AgentBrain<ActionData> brain){
        _canShake = true;
    }

    private void ChangeToIdle(AgentBrain<ActionData> brain){
        brain.ChangeState(StateType.Idle);
    }
}