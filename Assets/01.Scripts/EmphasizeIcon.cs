using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmphasizeIcon : PoolableMono
{
    public void Appear(float time, Action Callback = null)
    {
        StartCoroutine(AppearCoroutine(time,Callback));
    }
    private IEnumerator AppearCoroutine(float targetTime, Action Callback = null)
    {
        float timer = 0f;
        while (timer < targetTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one,Vector3.zero,timer * Time.deltaTime);
            yield return null;
        }
        Callback?.Invoke();
        PoolManager.Instance.Push(this);
    }
    public override void Init()
    {
        StopAllCoroutines();
    }
}
