using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Fade : BaseTransition
{
    [SerializeField] private float startAlpha = 0;
    [SerializeField] private float endAlpha = 1;
    [SerializeField] private CanvasGroup canvasGroup;

    public override async UniTask Run()
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float easedTime = easeCurve.Evaluate(t / duration);
            float progress = Mathf.LerpUnclamped(startAlpha, endAlpha, easedTime);
            canvasGroup.alpha = progress;
            await UniTask.Yield(canvasGroup.GetCancellationTokenOnDestroy());
        }
        canvasGroup.alpha = endAlpha;
    }
}
