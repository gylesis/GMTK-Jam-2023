using System;
using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts.UI
{
    public class Curtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private float _fadeDuration = 0.5f;

        public void FadeInOut(Action onFinish = null)
        {
            FadeIn((() => FadeOut(onFinish)));
        }
        
        public void FadeIn(Action onFade = null)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1, _fadeDuration).OnComplete((() => onFade?.Invoke()));
        }

        public void FadeOut(Action onFade = null)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0, _fadeDuration).OnComplete((() => onFade?.Invoke()));
        }
    }
}