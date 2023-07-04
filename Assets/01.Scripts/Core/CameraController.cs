using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Cinemachine;

public class CameraController{
    private CinemachineVirtualCamera _cmCam;
    private CinemachineVirtualCamera _talkCam;

    public CameraController(Transform trm){
        _cmCam = Define.CMCam;
        _talkCam = Define.TalkCam;

        _talkCam.enabled = false;
    }

    public void TalkMode(bool result){
        _talkCam.enabled = result;
        _cmCam.enabled = !result;
    }
}