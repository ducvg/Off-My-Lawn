using UnityEngine;

public interface IEquipment
{
    void Equip(Entity entity);
    void Remove();
}

public abstract class Equipment : MonoBehaviour, IEquipment
{
    protected Entity ownerEntity;

    public abstract void Equip(Entity entity);
    public abstract void Remove();  
}
