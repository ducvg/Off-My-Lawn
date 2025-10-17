using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Data Object/Equipment/Projectile Weapon Config")]
public class ProjectileWeaponConfigSO : WeaponConfigSO
{
    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public AnimationCurve FlyCurve { get; private set; }
}