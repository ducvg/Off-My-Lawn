using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [field: SerializeField] public Transform FirePoint { get; private set; }
    
}