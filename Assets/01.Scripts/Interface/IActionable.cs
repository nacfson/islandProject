using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionable {
    public void DoAction(AgentBrain<ActionData> brain);
    public void UnAction();
}