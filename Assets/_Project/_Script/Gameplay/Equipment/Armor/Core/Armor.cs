using UnityEngine;

public abstract class Armor : Equipment
{
    [field: SerializeField] public Renderer[] Renderers { get; private set; }
    public ArmorConfigSO Config { get; private set; }
    protected float health;

    public override void Equip(Entity entity)
    {
        foreach (var renderer in Renderers)
        {
            entity.GraphicController.AddOutfitRenderer(renderer);
        }
        health = Config.BaseHealth;
    }

    public abstract void Block(Entity entity, ref float damage);

    public override void Unequip(Entity entity)
    {
        foreach (var renderer in Renderers)
        {
            entity.GraphicController.RemoveOutfitRenderer(renderer);
        }
        Destroy(gameObject);
    }
}