using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Talk",fileName = "TalkData")]
public class TalkData : ScriptableObject {
    //�������� �������� string ���� ���� ����
    public List<string> talkList = new List<string>();
    public bool CanGetTalk(int returnIdx) {
        //return talkList[returnIdx] != null;
        return true;
    }
    public string GetTalk(int returnIdx) {
        string returnValue = talkList[returnIdx];
        return returnValue;
    }

}