using System.Collections.Generic;
using UnityEngine;

public class AnimClipOverrideList : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimClipOverrideList(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
