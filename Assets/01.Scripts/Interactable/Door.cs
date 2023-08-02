using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 _targetPos;
    private Transform _pivot;
    private Collider _col;

    private void Awake()
    {
        _pivot = transform.Find("DoorPivot");
        _col = GetComponent<Collider>();
    }

    public void Interact(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = brain as PlayerBrain;
        pb.GetAD().TargetPos = _pivot.position;
        pb.ChangeState(StateType.Entry);
        pb.AgentAnimator.OnOpenAnimationStartTrigger += OpenDoor;
    }
    public void OpenDoor(AgentBrain<ActionData> brain)
    {
        Debug.Log("OpenDoor");
        StartCoroutine(OpenDoorCor(brain));
    }
    IEnumerator OpenDoorCor(AgentBrain<ActionData> brain)
    {
        _col.enabled = false;
        float targetY = -90f; //밖에서 문 여는 경우임
        while (true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, targetY, 0)), Time.deltaTime);
            yield return null;
        }
    }

    public void UnInteract(AgentBrain<ActionData> brain)
    {
        //Debug.LogError("UnInteract");
        var pb = brain as PlayerBrain;
        pb.AgentMovement.SetPlayerPos(_targetPos);
        StopAllCoroutines();
        pb.AgentAnimator.OnOpenAnimationStartTrigger -= OpenDoor;
        Init();
    }

    private void Init()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _col.enabled = true;
    }
}