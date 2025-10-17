using UnityEngine;

public abstract class Armor : Equipment
{
    protected float health;

    public abstract void TakeDamage(float damage);
}