using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class GraphicController : MonoBehaviour
{
    [SerializeField] private List<Renderer> outfitRenderers;
    [SerializeField] private Renderer[] bodyRenderers;

    private Entity ownerEntity;

    public void Init(Entity owner)
    {
        ownerEntity = owner;
    }

    public void AddOutfitRenderer(Renderer renderer)
    {
        outfitRenderers.Add(renderer);
    }

    public void RemoveOutfitRenderer(Renderer renderer)
    {
        outfitRenderers.Remove(renderer);
    }

    public void ChangeMaterialAll(Material newMaterial)
    {
        foreach (var renderer in bodyRenderers) renderer.material = newMaterial;
        foreach (var renderer in outfitRenderers) renderer.material = newMaterial;
    }

    public void ChangeOutfitColor(Color newColor)
    {
        foreach (var renderer in outfitRenderers) renderer.material.color = newColor;
    }

    public Material GetHeroMaterial()
    {
        return bodyRenderers[0].material;
    }

#if UNITY_EDITOR
    [Button("Fetch Renderers")]
    private void FetchRenderersEditor()
    {
        bodyRenderers = GetComponentsInChildren<Renderer>();
        outfitRenderers = GetComponentsInChildren<Renderer>().ToList();
        EditorUtility.SetDirty(this);
    }
#endif
}