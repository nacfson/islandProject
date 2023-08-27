using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour{
    public bool IsRun;
    public bool IsInteracting;
    public bool IsPushing;
    public bool IsPicking;
    public bool UsingTool;

    public bool CanInteract;
    public bool CanAction;
    public Vector3 TargetPos;
}