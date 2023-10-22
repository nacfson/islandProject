using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
                _instance.Init();
            }
            return _instance;
        }
        
    }
    private CinemachineVirtualCamera _cmCam;
    private CinemachineVirtualCamera _talkCam;

    public void Generate(){}
    private void Init()
    {
        _cmCam = Define.CMCam;
        _talkCam = Define.TalkCam;

        _talkCam.enabled = false;
    }
    public void TalkMode(bool result)
    {
        _talkCam.enabled = result;
        _cmCam.enabled = !result;
    }
}