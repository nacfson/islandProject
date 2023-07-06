using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class ItemObject : MonoBehaviour{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _mat;

    [SerializeField] private Item _item;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;


    private void Awake() {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = _mat;
    }

    public Item GetItem(){
        if(_item != null) return _item;
        Debug.LogError("Item is not defined");
        return null;
    }

    public void SetItem(Item item){
        this._item = item;
    }




}