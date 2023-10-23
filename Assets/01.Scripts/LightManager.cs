using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    public override void Init(GameManager root)
    {
        base.Init(root);
        _light = _agentTrm.Find("MainLight").GetComponent<Light>();

        TimeManager.Instance.OnHourChanged += CalculateLight;
        TimeManager.Instance.OnHourChanged += HourChange;
    }

    private Light _light;

    [SerializeField] private AnimationCurve _fogDensityCurve;
    [SerializeField] private AnimationCurve _lightIntensityCurve;

    private float _curFogDensity;
    private float _curLightIntensity;


    private void HourChange(int hour)
    {
        Debug.Log(hour);
        //RenderSettings.fogDensity = _curFogDensity;
        //_light.intensity = _curLightIntensity;
    }

    private void CalculateLight(int hour)
    {
        float normalValue = (float)hour / 24.0f;
        _curFogDensity = _fogDensityCurve.Evaluate(normalValue);
        _curLightIntensity = _lightIntensityCurve.Evaluate(normalValue);
    }

    public void Generate() { }
}
