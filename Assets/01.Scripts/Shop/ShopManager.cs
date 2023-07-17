using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour{
    private static ShopManager _instance;
    public static ShopManager Instance => _instance;

    private void Awake(){
        if(_instance == null){
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


}