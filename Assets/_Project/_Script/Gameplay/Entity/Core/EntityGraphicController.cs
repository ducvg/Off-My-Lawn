using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class EntityGraphicController : MonoBehaviour
{
    [SerializeField] private List<Renderer> outfitRenderers;
    [SerializeField] private Renderer[] bodyRenderers;
    [field: SerializeField] public Animator Animator { get; private set; }
    public Material BodyMaterial { get; private set; }
    public List<Material> OutfitMaterials { get; private set; } = new();
    private AnimatorOverrideController animatorOverride;
    private AnimClipOverrideList overrideList;
    private Entity ownerEntity;
    private Sequence colorSequence;

    void Awake()
    {
        SetupMaterials();
        SetupAnimation();
    }

    public void Init(Entity owner)
    {
        ownerEntity = owner;
        SetEmissionAll(Color.black);
    }

    private void SetupMaterials()
    {
        BodyMaterial = bodyRenderers[0].material;
        int count = bodyRenderers.Length;
        for (int i = 0; i < count; i++)
        {
            bodyRenderers[i].material = BodyMaterial;
        }

        count = outfitRenderers.Count;
        for (int i = 0; i < count; i++)
        {
            OutfitMaterials.Add(outfitRenderers[i].material);
        }
    }

    private void SetupAnimation()
    {
        animatorOverride = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = animatorOverride;
        overrideList = new AnimClipOverrideList(animatorOverride.overridesCount);
        animatorOverride.GetOverrides(overrideList);
    }

    public EntityGraphicController WithOverrideAnimation(string keyClipName, AnimationClip newClip)
    {
        overrideList[keyClipName] = newClip;
        return this;
    }
    public void ApplyAnimatorOverrides()
    {
        animatorOverride.ApplyOverrides(overrideList);
    }
    public void PlayAnimation(int animHash, float crossFadeDuration = 0.1f)
    {
        Animator.CrossFade(animHash, crossFadeDuration);
    }
    public void AddOutfitRenderer(Renderer renderer)
    {
        outfitRenderers.Add(renderer);
        AddOutfitMaterial(renderer.material);
    }
    public void RemoveOutfitRenderer(Renderer renderer)
    {
        outfitRenderers.Remove(renderer);
        RemoveOutfitMaterial(renderer.material);
    }

    public void BlinkEmissionAll(in Color newColor, float duration)
    {
        colorSequence.Complete();
        colorSequence = Sequence.Create();
        colorSequence.Group(Tween.MaterialColor(BodyMaterial, GameConstant.emissionId, newColor, duration,
                ease: Ease.InCubic, cycleMode: CycleMode.Yoyo, cycles: 2));
        foreach (var mat in OutfitMaterials)
        {
            colorSequence.Group(Tween.MaterialColor(mat, GameConstant.emissionId, newColor, duration,
                    ease: Ease.InCubic, cycleMode: CycleMode.Yoyo, cycles: 2));
        }
    }

    public void SetEmissionAll(in Color newColor)
    {
        BodyMaterial.SetColor(GameConstant.emissionId, newColor);
        foreach (var mat in OutfitMaterials) mat.SetColor(GameConstant.emissionId, newColor);
    }
    
    public void SetOutfitColor(in Color newColor)
    {
        foreach (var mat in OutfitMaterials) mat.SetColor(GameConstant.colorId, newColor);
    }

    public void SetShaderAll(Shader newShader)
    {
        BodyMaterial.shader = newShader;
        foreach(var outfitMat in OutfitMaterials)
        {
            outfitMat.shader = newShader;
        }
    }
    public void SetBodyMaterial(Material material)
    {
        foreach (var renderer in bodyRenderers) renderer.material = material;
        BodyMaterial = material;
    }
    public void SetOutfitMaterials(List<Material> materials)
    {
        int count = OutfitMaterials.Count;
        for (int i = 0; i < count; i++)
        {
            outfitRenderers[i].material = materials[i];
        }
        OutfitMaterials = materials;
    }
    void AddOutfitMaterial(Material material)
    {
        OutfitMaterials.Add(material);
    }
    void RemoveOutfitMaterial(Material material)
    {
        OutfitMaterials.Remove(material);
    }

    void OnDestroy()
    {
        Destroy(animatorOverride);
        Destroy(BodyMaterial);
        foreach (var mat in OutfitMaterials)
        {
            Destroy(mat);
        }
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