using System;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public interface ITransition
{
    public Tween Run();
}

[Serializable]
public class TransitionData
{
    [SerializeReference] private ITransition[] openTransitions;
    [SerializeReference] private ITransition[] closeTransitions;

    public async UniTask Open(MonoBehaviour caller)
    {
        Sequence sequence = Sequence.Create();
        foreach (var transition in openTransitions)
        {
            _ = sequence.Chain(transition.Run()); //ignore await warning
        }
        await sequence.WithCancellation(caller.destroyCancellationToken);
    }

    public async UniTask Close(MonoBehaviour caller)
    {
        Sequence sequence = Sequence.Create();
        foreach (var transition in closeTransitions)
        {
            _ = sequence.Chain(transition.Run()); //ignore await warning
        }
        await sequence.WithCancellation(caller.destroyCancellationToken);
    }
}

