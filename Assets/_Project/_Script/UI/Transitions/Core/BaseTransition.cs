using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public abstract class BaseTransition
{
    [SerializeField] protected float duration = 0.5f;
    [SerializeField] protected AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public abstract UniTask Run(); //async
}
