using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Core{
    public class Define
    {
        private static Camera _mainCam;
        public static Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                {
                    _mainCam = Camera.main;
                }
                return _mainCam;
            }
        }
        
        private static CinemachineVirtualCamera _cmCam;
        public static CinemachineVirtualCamera CMCam
        {
            get
            {
                if(_cmCam== null)
                {
                    _cmCam = GameObject.Find("CMCam").GetComponent<CinemachineVirtualCamera>();
                }
                return _cmCam;
            }
        }

        private static CinemachineVirtualCamera _talkCam;
        public static CinemachineVirtualCamera TalkCam
        {
            get
            {
                if(_talkCam == null)
                {
                    _talkCam = GameObject.Find("TalkCam").GetComponent<CinemachineVirtualCamera>();
                }
                return _talkCam;
            }
        }



    }

}