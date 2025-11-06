using PrimeTween;
using UnityEngine;

public class GenericShield : Shield
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;
    private Tween emissionTween;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        material = meshRenderer.material;
    }

    public override void Block(Entity entity, ref float damage)
    {
        entity.GraphicController.PlayAnimation(Animation.HurtHash, 0.1f);
        BlinkEmission();
        
        float leftoverDamage = damage - health;
        health -= damage;
        damage = leftoverDamage > 0 ? leftoverDamage : 0;

        if (health <= 0)
        {
            Unequip(entity);
        }
    }

    void BlinkEmission()
    {
        emissionTween.Complete();
        emissionTween = Tween.MaterialColor(material, GameConstant.emissionId, new Color(0.2f, 0.2f, 0.2f, 1f),
                                        duration: 0.15f, Ease.InCubic, cycles: 2, cycleMode: CycleMode.Yoyo);
    }

    public override void Unequip(Entity entity)
    {
        base.Unequip(entity);
        Destroy(material);
    }

    private void OnDestroy()
    {
        if(material) Destroy(material);
    }
}