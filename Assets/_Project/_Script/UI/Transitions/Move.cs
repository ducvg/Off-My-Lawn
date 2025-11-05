using System;
using System.Runtime.CompilerServices;
using PrimeTween;
using UnityEngine;

[Serializable]
public class Move : ITransition
{
    [SerializeField] private TweenSettings<Vector3> settings;
    [SerializeField] private Transform target;

    public Tween Run()
    {
        return Tween.Position(target, settings);
    }
}