using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Move : BaseTransition
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Transform target;

    public override async UniTask Run()
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float easedTime = easeCurve.Evaluate(t / duration);
            Vector3 progress = Vector3.LerpUnclamped(startPosition, endPosition, easedTime);
            target.localPosition = progress;
            await UniTask.Yield(target.GetCancellationTokenOnDestroy());
        }
        target.localPosition = endPosition;
    }
}