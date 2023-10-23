using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : Singleton<CameraManager>
{
    private CinemachineVirtualCamera _cmCam;
    private CinemachineVirtualCamera _talkCam;
    public override void Init(GameManager root)
    {
        base.Init(root);
        _cmCam = Define.CMCam;
        _talkCam = Define.TalkCam;
        
        _talkCam.enabled = false;
    }
    public void TalkMode(bool result)
    {
        if (_talkCam == null || _cmCam == null)
        {
            _talkCam = Define.TalkCam;
            _cmCam = Define.CMCam;  
        }
        if(_talkCam != null)
            _talkCam.enabled = result;
        if(_cmCam != null)
            _cmCam.enabled = !result;
    }

}