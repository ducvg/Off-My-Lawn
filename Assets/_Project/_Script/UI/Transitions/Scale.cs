using System;
using System.Runtime.CompilerServices;
using PrimeTween;
using UnityEngine;

[Serializable]
public class Scale : ITransition
{
    [SerializeField] private TweenSettings<Vector3> settings;
    [SerializeField] private RectTransform target;

    public Tween Run()
    {
        return Tween.Scale(target, settings);
    }
}
