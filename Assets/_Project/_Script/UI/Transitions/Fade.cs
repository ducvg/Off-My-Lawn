using System;
using System.Runtime.CompilerServices;
using PrimeTween;
using UnityEngine;

[Serializable]
public class Fade : ITransition
{
    [SerializeField] private TweenSettings<float> settings;
    [SerializeField] private CanvasGroup canvasGroup;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tween Run()
    {
        return Tween.Alpha(canvasGroup, settings);
    }
}
