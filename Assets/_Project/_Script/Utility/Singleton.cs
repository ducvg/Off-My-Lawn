using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Find singleton
                instance = FindAnyObjectByType<T>();

                // // Create new instance if one doesn't already exist.
                // if (instance == null)
                // {
                //     // Need to create a new GameObject to attach the singleton to.
                //     instance = new GameObject(typeof(T).Name).AddComponent<T>();
                // }
            }
            return instance;
        }
        protected set => instance = value;
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this as T;
    }
}

public class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}