using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Movement",fileName = "MovementData")]
public class MovementData : ScriptableObject{
    public float Speed = 5f;
    public float RunSpeed = 8f;
    public float entrySpeed = 1f;

    //public float MaxSpeed;
}