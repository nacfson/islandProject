using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : Singleton<UIManager>
{
    public override void Init(GameManager root)
    {
        base.Init(root);
    }

    [SerializeField] private Image _fadeUI;
    private Vector3 _maxOffset = new Vector3(1.2f, 2.0f, 1f);
    
    public void FadeSequence(float time = 2f,Action MiddleAction = null)
    {
        _fadeUI.rectTransform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeUI.rectTransform.DOScale(_maxOffset, time).SetEase(Ease.InOutCubic));
        seq.AppendCallback(() => MiddleAction?.Invoke());
        seq.Append(_fadeUI.rectTransform.DOScale(Vector3.zero,time).SetEase(Ease.InOutCubic));
    }

}
