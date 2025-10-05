using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class TransitionData
{
    [SerializeReference, Subclass] private BaseTransition[] openTransitions;
    [SerializeReference, Subclass] private BaseTransition[] closeTransitions;

    public async UniTask Open()
    {
        List<UniTask> transitionTasks = new();
        foreach (var transition in openTransitions)
        {
            transitionTasks.Add(transition.Run());
        }
        await UniTask.WhenAll(transitionTasks);
    }

    public async UniTask Close()
    {
        List<UniTask> transitionTasks = new();
        foreach (var transition in closeTransitions)
        {
            transitionTasks.Add(transition.Run());
        }
        await UniTask.WhenAll(transitionTasks);
    }
}

