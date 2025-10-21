using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected TransitionData transitionData;

    protected bool isTransitioning = false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Setup()
    {
    }

    //called after opening the canvas
    [Button("Open Canvas")]
    public virtual async UniTask Open()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        gameObject.SetActive(true);
        await transitionData.Open(this);

        isTransitioning = false;
    }

    //delay closing the canvas
    [Button("Close Canvas")]
    public virtual async UniTask Close()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        await transitionData.Close(this);
        gameObject.SetActive(false);

        isTransitioning = false;
    }

    public virtual void CloseImmediate()
    {
        if (isTransitioning) return;
        gameObject.SetActive(false);
        isTransitioning = false;
    }
}