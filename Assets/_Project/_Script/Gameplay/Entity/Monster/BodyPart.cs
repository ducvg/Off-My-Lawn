using System;
using UnityEngine;

[Serializable]
public class BodyPart
{
    [field: SerializeField, Range(0, 1)] public float BreakThreshold { get; private set; }
    [SerializeField] private Transform rootTransform;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshes;
    public bool IsDetached { get; private set; } = false;

    public void Init()
    {
        int skinCount = skinnedMeshes.Length;
        for (int i = 0; i < skinCount; ++i)
        {
            skinnedMeshes[i].enabled = true;
        }
        IsDetached = false;
    }

    public void BreakOff(float flingForce)
    {
        int skinCount = skinnedMeshes.Length;
        for (int i = 0; i < skinCount; ++i)
        {
            skinnedMeshes[i].enabled = false;
        }
        IsDetached = true;

        DetachObject fakePart = DetachObjectFactory.Instance.Spawn(skinnedMeshes[0], rootTransform.position, rootTransform.rotation);
        fakePart.Fling(flingForce);
    }
}
