using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_Toolkit;


public abstract class AIInteractable : Agent<AIActionData>, IInteractable{

    public abstract void Interact(AgentBrain<ActionData> brain);

    public abstract void UnInteract(AgentBrain<ActionData> brain);
}