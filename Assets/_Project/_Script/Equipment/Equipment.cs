using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    public abstract void Equip(Entity entity);
    public abstract void Unequip(Entity entity);
}
