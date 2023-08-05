using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FishingRod : MonoBehaviour, IActionable
{
    private Transform _bobber;

    private void Awake()
    {
        _bobber = transform.Find("Bobber");
    }
    public void DoAction(AgentBrain<ActionData> brain)
    {
        ThrowBobber();
    }

    public void UnAction(AgentBrain<ActionData> brain)
    {

    }

    public void ThrowBobber()
    {
        Vector3 endValue = Vector3.zero;
        float jumpPower = 2f;
        int numJumps = 1;
        float duration = 2f;
        bool snapping = false;

        _bobber.DOJump(endValue,jumpPower,numJumps,duration,snapping);

    }

}
