using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform canvasRoot;
    [SerializeField] private List<BaseCanvas> prefabList;

    private Dictionary<Type, BaseCanvas> activeCanvases = new();
    private Dictionary<Type, BaseCanvas> canvasPrefabs = new();

    private void Awake()
    {
        foreach (var canvas in prefabList)
        {
            canvasPrefabs.Add(canvas.GetType(), canvas);
        }
        prefabList = null; //dipose the list
    }

    public T Open<T>() where T : BaseCanvas
    {
        T canvas = GetCanvas<T>();

        canvas.Setup();
        canvas.Open().Forget();

        return canvas;
    }

    public async UniTask<T> OpenAsync<T>() where T : BaseCanvas
    {
        T canvas = GetCanvas<T>();
        canvas.Setup();
        await canvas.Open();
        return canvas;
    }

    public void Close<T>() where T : BaseCanvas
    {
        if (IsOpened<T>())
        {
            activeCanvases[typeof(T)].Close().Forget();
        }
    }

    public async UniTask CloseAsync<T>() where T : BaseCanvas
    {
        if (IsOpened<T>())
        {
            await activeCanvases[typeof(T)].Close();
        }
    }

    public void CloseImmediate<T>() where T : BaseCanvas
    {
        if (IsOpened<T>())
        {
            activeCanvases[typeof(T)].CloseImmediate();
        }
    }

    public T GetCanvas<T>() where T : BaseCanvas
    {
        if (!IsLoaded<T>())
        {
            T canvas = Instantiate(GetCanvasPrefab<T>(), canvasRoot);
            activeCanvases[typeof(T)] = canvas;
        }
        return activeCanvases[typeof(T)] as T;
    }

    public bool IsLoaded<T>() where T : BaseCanvas
    {
        return activeCanvases.ContainsKey(typeof(T)) && activeCanvases[typeof(T)] != null;
    }

    public bool IsOpened<T>() where T : BaseCanvas
    {
        return IsLoaded<T>() && activeCanvases[typeof(T)].gameObject.activeSelf;
    }

    private T GetCanvasPrefab<T>() where T : BaseCanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    public void CloseAll()
    {
        foreach (var canvas in activeCanvases)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close().Forget();
            }
        }
    }
}