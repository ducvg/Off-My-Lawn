using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Data Object/Equipment/Projectile Weapon Config")]
public class ProjectileWeaponConfigSO : WeaponConfigSO
{
    [field: Header("Projectile")]
    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; } = AnimationCurve.Linear(0, 1, 1, 1);
}