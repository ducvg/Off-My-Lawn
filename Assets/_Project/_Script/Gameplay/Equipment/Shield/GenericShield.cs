using PrimeTween;
using UnityEngine;

public class GenericShield : Shield
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;
    private Tween blinkTween;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        material = meshRenderer.material;
    }

    public override void Block(Entity entity, ref float damage)
    {
        entity.GraphicController.PlayAnimation(Animation.HurtHash, 0.1f);
        BlinkEmission();
        
        var tmp = health;
        health -= damage;
        damage -= tmp;

        if (health <= 0)
        {
            Unequip(entity);
        }
    }

    void BlinkEmission()
    {
        blinkTween.Complete();
        blinkTween = Tween.MaterialColor(material, GameConstant.emissionId, new Color(0.2f, 0.2f, 0.2f, 1f),
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