using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public sealed class Move : ITransition
{
    [SerializeField] private TweenSettings<Vector2> settings;
    [SerializeField] private RectTransform target;

    public Tween Run()
    {
        return Tween.UIAnchoredPosition(target, settings);
    }
}