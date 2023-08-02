using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private static LightManager _instance;
    public static LightManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LightManager>();
            }
            return _instance;
        }
    }
    private Light _light;

    [SerializeField] private AnimationCurve _fogDensityCurve;
    [SerializeField] private AnimationCurve _lightIntensityCurve;

    private float _curFogDensity;
    private float _curLightIntensity;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        _light = transform.Find("MainLight").GetComponent<Light>();

        TimeManager.Instance.OnHourChanged += CalculateLight;
        TimeManager.Instance.OnHourChanged += HourChange;
    }

    private void HourChange(int hour)
    {
        Debug.Log(hour);
        RenderSettings.fogDensity = _curFogDensity;
        _light.intensity = _curLightIntensity;
    }

    private void CalculateLight(int hour)
    {

    }

    public void Generate(){}
}
