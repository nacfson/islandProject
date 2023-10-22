using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AgentTool : PlayerAgent
{
    [SerializeField] private Transform _toolTrm;
    public Transform ToolTrm => _toolTrm;
    private TwoBoneIKConstraint _twoBone;
    public override void SetUp(Transform agent)
    {
        base.SetUp(agent);
        _twoBone = _brain.transform.Find($"Visual/RigLayer/RightHandIK").GetComponent<TwoBoneIKConstraint>();

        for(int i = 0;  i < _toolTrm.childCount; i++)
        {
            GameObject toolObj = _toolTrm.GetChild(i).gameObject;
            Debug.Log(string.Format("ToolObjectName: {0}",toolObj.name));
            if (toolObj.TryGetComponent<ITool>(out ITool tool))
            {
                tool.Init(this.transform);
            }
            toolObj.SetActive(false);
        }
    }
    
    public void SetToolActive(string name = "*", bool active = false)
    {
        if (active)
        {
            _twoBone.data.target = _toolTrm;
        }
        else
        {
            _twoBone.data.target = null;
        }

        if (name == "*")
        {
            for(int i = 0;  i < _toolTrm.childCount; i++)
            {
                GameObject toolObj = _toolTrm.GetChild(i).gameObject;
                toolObj.SetActive(false);
            }
        }
        else
        {
            GameObject toolObj = _toolTrm.Find($"{name}").gameObject;
            Debug.Log($"ToolObjName: {(toolObj).gameObject.name}");
            if(toolObj == null) Debug.LogError($"Can't Find at {name}, toolObj is Null{toolObj}");
            toolObj.SetActive(active);
        }

    }
    
}
