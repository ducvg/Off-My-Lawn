using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public class Rotation : ITransition
{
    [SerializeField] private TweenSettings<Quaternion> settings;
    [SerializeField] private RectTransform target;

    public Tween Run()
    {
        return Tween.Rotation(target, settings);
    }

}
