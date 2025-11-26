using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public sealed class Scale : ITransition
{
    [SerializeField] private TweenSettings<Vector3> settings;
    [SerializeField] private RectTransform target;

    public Tween Run()
    {
        return Tween.Scale(target, settings);
    }
}
