using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Rotation : BaseTransition
{
    [SerializeField] private Vector3 startRotation = Vector3.zero;
    [SerializeField] private Vector3 endRotation = new(360, 360, 360);
    [SerializeField] private Transform target;

    public override async UniTask Run()
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float easedTime = easeCurve.Evaluate(t / duration);
            Vector3 progress = Vector3.LerpUnclamped(startRotation, endRotation, easedTime);
            target.rotation = Quaternion.Euler(progress);
            await UniTask.Yield(target.GetCancellationTokenOnDestroy());
        }
        target.rotation = Quaternion.Euler(endRotation);
    }

}
