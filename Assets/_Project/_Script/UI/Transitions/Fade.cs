using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public sealed class Fade : ITransition
{
    [SerializeField] private TweenSettings<float> settings;
    [SerializeField] private CanvasGroup canvasGroup;

    public Tween Run()
    {
        return Tween.Alpha(canvasGroup, settings);
    }
}
