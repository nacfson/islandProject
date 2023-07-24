using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Collider))]
public class ItemObject : PoolableMono, IInteractable
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _mat;

    [SerializeField] private Item _item;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;


    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = _mat;
    }

    public Item GetItem()
    {
        return _item;
    }

    public void SetItem(Item item)
    {
        this._item = item;
    }

    public override void Init()
    {
        Debug.Log("Init");

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = _mat;
    }

    public void Interact(AgentBrain<ActionData> brain)
    {
        PoolManager.Instance.Push(this);
        Debug.Log("Interact ItemObj");
        PlayerBrain pb = brain as PlayerBrain;
        pb.ChangeState(StateType.Pick);
        pb.AgentAnimator.OnPickAnimationEndTrigger += ChangeToIdle;

        //기회가되면 더 깔끔하게 코드 구조를 바꿔야함\
        InventoryManager.Instance.AddItem(GetItem(), 1);
    }

    public void UnInteract(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPickAnimationEndTrigger -= ChangeToIdle;
    }

    public void ChangeToIdle(AgentBrain<ActionData> brain)
    {
        brain.ChangeState(StateType.Idle);
    }
}