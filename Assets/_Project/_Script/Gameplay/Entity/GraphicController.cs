using UnityEditor;
using UnityEngine;

public class GraphicController : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    public void Init()
    {
    }

    public void ChangeMaterial(Material newMaterial)
    {
        foreach (var renderer in renderers)
        {
            renderer.material = newMaterial;
        }
    }

    public Material GetHeroMaterial()
    {
        return renderers[0].material;
    }

#if UNITY_EDITOR
    [ContextMenu("Fetch Renderers")]
    private void FetchRenderersEditor()
    {
        renderers = GetComponentsInChildren<Renderer>();
        EditorUtility.SetDirty(this);
    }
#endif
}