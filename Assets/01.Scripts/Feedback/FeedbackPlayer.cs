using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FeedbackPlayer : MonoBehaviour
{
    [SerializeField] private List<Feedback> feedbackList = new List<Feedback>();
    public void PlayFeedback()
    {
        FinishFeedback();
        foreach(var f in feedbackList)
        {
            f.CreateFeedback();
        }
    }
    public void FinishFeedback()
    {
        foreach(var f in feedbackList)
        {
            f.CompleteFeedback();
        }
    }
}
