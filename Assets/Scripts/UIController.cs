using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public RectTransform buttonHealth;
    void Start()
    {
        DoMove();
    }

    public void DoMove()
    {
        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(buttonHealth.DOAnchorPosX(1000, 0.05f).SetEase(Ease.Linear));
        moveSequence.Append(buttonHealth.DOAnchorPosX(1020, 0.05f).SetEase(Ease.Linear));
        moveSequence.SetLoops(5);
    }
}
