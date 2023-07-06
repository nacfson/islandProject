using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour,IInteractable{
    private int _shakeCount = 3;
    [SerializeField] private float _shakeTimer;

    public List<ItemObject> items = new List<ItemObject>();
    

    public ItemObject GetRandItemObj(){
        int rand = Random.Range(0,items.Count);
        return items[rand];
    }

    public void Interact(AgentBrain<ActionData> brain){
        Debug.Log("Interact");
        brain.ChangeState(StateType.Push);
        //audience.transform.Find("Visual").GetComponent<AgentAnimator>().SetBoolPush(true);
    }

    public void UnInteract(){
        _shakeCount = 3;
    }

    public void Shake(AgentBrain<ActionData> brain){
        _shakeCount--;
        _shakeCount = Mathf.Clamp(_shakeCount,0,3);
        if(_shakeCount <= 0){
            Debug.Log("DropItem");
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
}