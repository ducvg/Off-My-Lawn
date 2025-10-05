using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Scale : BaseTransition
{
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private Transform target;

    public override async UniTask Run()
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float easedTime = easeCurve.Evaluate(t / duration);
            Vector3 progress = Vector3.LerpUnclamped(startScale, endScale, easedTime);
            target.localScale = progress;
            await UniTask.Yield(target.GetCancellationTokenOnDestroy());
        }
        target.localScale = endScale;
    }
}
