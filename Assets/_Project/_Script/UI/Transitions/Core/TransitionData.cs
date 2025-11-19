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
        int length = openTransitions.Length;
        for (int i = 0; i < length; i++)
        {
            sequence.Group(openTransitions[i].Run());
        }
        return sequence;
    }

    public Sequence Close()
    {
        Sequence sequence = Sequence.Create();
        int length = closeTransitions.Length;
        for (int i = 0; i < length; i++)
        {
            sequence.Group(closeTransitions[i].Run());
        }
        return sequence;
    }
}

