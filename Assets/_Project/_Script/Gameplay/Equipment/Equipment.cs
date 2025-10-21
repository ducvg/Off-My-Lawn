using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    [field: SerializeField] public Renderer[] Renderers { get; private set; }
    protected Entity ownerEntity;

    public virtual void Equip(Entity entity)
    {
        ownerEntity = entity;
        foreach (var renderer in Renderers)
        {
            ownerEntity.GraphicController.AddOutfitRenderer(renderer);
        }
    }
    
    public virtual void Unequip()
    {
        foreach (var renderer in Renderers)
        {
            ownerEntity.GraphicController.RemoveOutfitRenderer(renderer);
        }
        Destroy(gameObject);
    }
}
