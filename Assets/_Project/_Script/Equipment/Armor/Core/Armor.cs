using UnityEngine;

public abstract class Armor : Equipment
{
    [SerializeField] protected MeshFilter meshFilter;
    [SerializeField] protected new Renderer renderer;
    public ArmorConfigSO Config { get; protected set; }
    public Material Material { get;  protected set; }
    protected float health;

    void Awake()
    {
        Material = renderer.material;
    }

    public void SetConfig(ArmorConfigSO config)
    {
        Config = config;
    }

    public override void Equip(Entity entity)
    {
        health = Config.BaseHealth;
    }

    public abstract void Block(Entity entity, ref float damage);

    public override void Unequip(Entity entity)
    {
        entity.EquipmentController.UnequipArmor(this);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Destroy(renderer.material);
    }
}