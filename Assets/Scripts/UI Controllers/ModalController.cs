using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalController : MonoBehaviour
{
    public bool OpenOnStart;

    public Image blackBackground;
    public RectTransform modal;

    public UnityEvent onStartTransition;
    public UnityEvent onEndTransition;
    public UnityEvent onOpen;
    public UnityEvent onClose;

    private bool isOpen;

    void Start()
    {
        CloseModal();

        if (OpenOnStart)
            OpenModal();
    }

    public void OpenModal()
    {
        if (isOpen)
            return;

        isOpen = true;

        blackBackground.raycastTarget = true;
        FadeIn();
        onOpen.Invoke();
    }

    public void CloseModal()
    {
        if (!isOpen)
            return;

        isOpen = false;

        blackBackground.raycastTarget = false;
        FadeOut();

    }

    private void FadeIn(float duration = 0.5f, Action beginFadeAction = null, Action endFadeAction = null)
    {
        StartCoroutine(Fade(1, duration, beginFadeAction, endFadeAction));
    }

    private void FadeOut(float duration = 0.5f, Action beginFadeAction = null, Action endFadeAction = null)
    {
        StartCoroutine(Fade(0, duration, beginFadeAction, endFadeAction));
    }

    IEnumerator Fade(float scale, float duration, Action beginFadeAction = null, Action endFadeAction = null)
    {
        onStartTransition.Invoke();

        beginFadeAction?.Invoke();
        blackBackground.DOFade(scale * 0.6f, duration);
        Tweener t = modal.DOScale(Vector3.one * scale, duration);
        yield return t.WaitForCompletion();

        onEndTransition.Invoke();

        endFadeAction?.Invoke();

        if (scale == 0)
        {
            onClose.Invoke();
        }
    }
}
