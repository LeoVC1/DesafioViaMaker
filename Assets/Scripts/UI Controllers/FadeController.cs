using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class FadeController : MonoBehaviour
{
    CanvasGroup canvas;

    public UnityEvent onFinishFade;

    public void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void DOFade(bool fadeIn)
    {
        canvas.DOFade(fadeIn ? 1 : 0, 1).OnComplete(delegate {
            canvas.blocksRaycasts = fadeIn;
            canvas.interactable = fadeIn;
            onFinishFade.Invoke();
        });
    }
}
