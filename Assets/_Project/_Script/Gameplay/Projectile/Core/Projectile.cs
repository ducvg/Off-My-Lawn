using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Weapon OwnerWeapon { get; private set; }
    public Vector3 LastPosition { get; private set; }
    protected ProjectileConfigSO config;
    protected int pierceCount;

    public virtual void Init(Weapon weapon)
    {
        OwnerWeapon = weapon;
        LastPosition = transform.position;
        config = weapon.Config.ProjectileConfig;
        pierceCount = weapon.Config.AttackPierce;
    }

    public virtual void Update()
    {
        LastPosition = transform.position;
    }

    public virtual void OnHit(Entity target)
    {
        foreach (var effect in OwnerWeapon.Config.AttackEffects)
        {
            effect.Apply(target);
        }
        if (--pierceCount <= 0)
        {
            Despawn();
        }
    }

    public virtual void Despawn()
    {
        ProjectileManager.Instance.Release(OwnerWeapon.Config.ProjectileConfig.Prefab, this);
    }
}