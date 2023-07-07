using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour,IInteractable,IActionable{
    private int _shakeCount = 3;
    [SerializeField] private float _shakeTimer = 3f;
    [SerializeField] private ItemListData _itemList;

    private bool _canDrop = true;


    public void Interact(AgentBrain<ActionData> brain){
        Debug.Log("Interact");
        brain.ChangeState(StateType.Push);
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
        if (!_canDrop) return;
        _shakeCount--;
        _shakeCount = Mathf.Clamp(_shakeCount,0,3);
        if(_shakeCount <= 0){
            DropItem();
            brain.ChangeState(StateType.Idle);
            StopAllCoroutines();
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ShakeCor(brain));
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
    public void UnAction() {
        _shakeCount = 3;
    }



}