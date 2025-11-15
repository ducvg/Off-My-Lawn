using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Weapon OwnerWeapon { get; private set; }
    public Vector3 LastPosition { get; private set; }
    protected ProjectileConfigSO config;
    protected int pierceCount;
    protected Entity lastHitTarget;

    public virtual void Init(Weapon weapon)
    {
        OwnerWeapon = weapon;
        LastPosition = transform.position;
        config = weapon.Config.ProjectileConfig;
        pierceCount = weapon.Config.AttackPierce;
        lastHitTarget = null;
    }

    public virtual void Update()
    {
        LastPosition = transform.position;
    }

    public virtual void OnHit(Entity target)
    {
        if (target == lastHitTarget) return; //raycast can hit target again if pierce
        lastHitTarget = target;

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