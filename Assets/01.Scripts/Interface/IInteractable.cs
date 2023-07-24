using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(AgentBrain<ActionData> brain);
    public void UnInteract(AgentBrain<ActionData> brain);
}