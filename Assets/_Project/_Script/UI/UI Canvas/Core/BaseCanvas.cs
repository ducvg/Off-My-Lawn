using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;

public abstract class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected TransitionData transitionData;
    protected bool isTransitioning = false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Setup()
    {
    }

    public virtual async UniTask OpenAsync()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        gameObject.SetActive(true);
        await transitionData.Open();

        isTransitioning = false;
    }

    public virtual async UniTask CloseAsync()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        await transitionData.Close();
        gameObject.SetActive(false);

        isTransitioning = false;
    }

    public virtual void OpenImmediate()
    {
        if (isTransitioning) return;

        gameObject.SetActive(true);
        transitionData.Open().Complete();
        isTransitioning = false;
    }

    public virtual void CloseImmediate()
    {
        if (isTransitioning) return;

        transitionData.Close().Complete();
        gameObject.SetActive(false);
        isTransitioning = false;
    }
}