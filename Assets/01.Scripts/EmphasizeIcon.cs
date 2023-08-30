using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmphasizeIcon : PoolableMono
{
    public void Appear(float time, Action Callback)
    {
        StartCoroutine(AppearCoroutine(time,Callback));
    }
    private IEnumerator AppearCoroutine(float time, Action Callback)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
        PoolManager.Instance.Push(this);
    }
    public override void Init()
    {
        StopAllCoroutines();
    }
}
