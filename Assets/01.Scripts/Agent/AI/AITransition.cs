using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition : MonoBehaviour {
    [HideInInspector] public List<AIDecision> decisions;
    public NormalAIState NextState;
    public void SetUp(Transform agent) {
        decisions = new List<AIDecision>();
        GetComponents<AIDecision>(decisions);

        decisions.ForEach(d => d.SetUp(agent));
    }

    public bool MakeATransition() {
        bool result = false;
        foreach(AIDecision d in decisions) {
            result = d.MakeADecision();
            if (d.IsReverse) {
                result = !result;
            }
            if (!result) break;
        }
        return result;
    }
}