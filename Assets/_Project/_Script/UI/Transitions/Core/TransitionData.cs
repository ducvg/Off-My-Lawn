using System;
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

    public Sequence Open()
    {
        Sequence sequence = Sequence.Create();
        for (int i = 0; i < openTransitions.Length; i++)
        {
            sequence.Group(openTransitions[i].Run());
        }
        return sequence;
    }

    public Sequence Close()
    {
        Sequence sequence = Sequence.Create();
        for (int i = 0; i < closeTransitions.Length; i++)
        {
            sequence.Group(closeTransitions[i].Run());
        }
        return sequence;
    }
}

