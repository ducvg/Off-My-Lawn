using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected TransitionData transitionData;

    protected bool isTransitioning = false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Setup()
    {
    }

    //called after opening the canvas
    public virtual async UniTask Open()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        gameObject.SetActive(true);
        await transitionData.Open();

        isTransitioning = false;
    }

    //delay closing the canvas
    public virtual async UniTask Close()
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
        transitionData.OpenImmediate();
        isTransitioning = false;
    }

    public virtual void CloseImmediate()
    {
        if (isTransitioning) return;
        transitionData.CloseImmediate();
        gameObject.SetActive(false);
        isTransitioning = false;
    }
}