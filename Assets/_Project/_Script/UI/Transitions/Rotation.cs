using System;
using System.Runtime.CompilerServices;
using PrimeTween;
using UnityEngine;

[Serializable]
public class Rotation : ITransition
{
    [SerializeField] private TweenSettings<Quaternion> settings;
    [SerializeField] private Transform target;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tween Run()
    {
        return Tween.Rotation(target, settings);
    }

}
