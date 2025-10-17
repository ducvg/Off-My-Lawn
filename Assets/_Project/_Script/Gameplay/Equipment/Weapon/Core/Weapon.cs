using UnityEngine;

public abstract class Weapon : Equipment
{
    public abstract void Attack(Entity target);
}