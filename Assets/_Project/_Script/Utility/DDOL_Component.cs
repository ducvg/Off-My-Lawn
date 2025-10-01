using UnityEngine;

public class DDOL_Component : MonoBehaviour
{
    protected void Awake()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
}