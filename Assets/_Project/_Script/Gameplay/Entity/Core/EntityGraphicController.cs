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
    private Material bodyMaterial;
    private HashSet<Material> outfitMaterials = new();
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
        bodyMaterial = bodyRenderers[0].material;
        int count = bodyRenderers.Length;
        for (int i = 0; i < count; i++)
        {
            bodyRenderers[i].material = bodyMaterial;
        }

        count = outfitRenderers.Count;
        for (int i = 0; i < count; i++)
        {
            outfitMaterials.Add(outfitRenderers[i].material);
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

    public void BlinkEmissionAll(in Color newColor, float duration)
    {
        colorSequence.Complete();
        colorSequence = Sequence.Create();
        colorSequence.Group(Tween.MaterialColor(bodyMaterial, ShaderId.emission, newColor, duration,
                ease: Ease.InCubic, cycleMode: CycleMode.Yoyo, cycles: 2));
        foreach (var mat in outfitMaterials)
        {
            colorSequence.Group(Tween.MaterialColor(mat, ShaderId.emission, newColor, duration,
                    ease: Ease.InCubic, cycleMode: CycleMode.Yoyo, cycles: 2));
        }
    }

    public void SetEmissionAll(in Color newColor)
    {
        bodyMaterial.SetColor(ShaderId.emission, newColor);
        foreach (var mat in outfitMaterials) mat.SetColor(ShaderId.emission, newColor);
    }
    
    public void SetOutfitColor(in Color newColor)
    {
        foreach (var mat in outfitMaterials) mat.SetColor(ShaderId.color, newColor);
    }

    public void SetBodyMaterial(Material material)
    {
        foreach (var renderer in bodyRenderers) renderer.material = material;
        bodyMaterial = material;
    }

    public void AddOutfitMaterial(Material material)
    {
        outfitMaterials.Add(material);
    }

    public void RemoveOutfitMaterial(Material material)
    {
        outfitMaterials.Remove(material);
    }

    public void SetShaderAll(Shader newShader)
    {
        bodyMaterial.shader = newShader;
        foreach (var outfitMat in outfitMaterials)
        {
            outfitMat.shader = newShader;
        }
    }
    
    void OnDestroy()
    {
        Destroy(animatorOverride);
        Destroy(bodyMaterial);
        foreach (var mat in outfitMaterials)
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