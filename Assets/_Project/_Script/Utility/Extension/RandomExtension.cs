using System;
using System.Collections.Generic;

public static class RandomExtension
{
    public static int GetRandomWeightedIndex(this List<int> weights)
    {
        int resultIndex = -1;
        float totalWeight = 0;
        int i; int count = weights.Count;
        for (i = 0; i < count; ++i)
        {
            float evaluatingWeight = weights[i];

            if (evaluatingWeight <= 0f) continue;
            float random = UnityEngine.Random.Range(0f, totalWeight + evaluatingWeight);

            if (random >= totalWeight)
            {
                resultIndex = i;
            }

            totalWeight += evaluatingWeight;
        }
        return resultIndex;
    }

    public static int GetRandomWeightedIndex(this Span<float> weights)
    {
        int resultIndex = -1;
        float totalWeight = 0;
        int i; int count = weights.Length;
        for (i = 0; i < count; ++i)
        {
            float evaluatingWeight = weights[i];

            if (evaluatingWeight <= 0f) continue;
            float random = UnityEngine.Random.Range(0f, totalWeight + evaluatingWeight);

            if (random >= totalWeight)
            {
                resultIndex = i;
            }

            totalWeight += evaluatingWeight;
        }
        return resultIndex;
    }
}