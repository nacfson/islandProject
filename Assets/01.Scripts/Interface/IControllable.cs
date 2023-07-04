using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable {
    public void ChangeState(object state); 
    public void SetUp(Transform agent);



}