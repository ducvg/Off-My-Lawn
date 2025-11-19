using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Weapon OwnerWeapon { get; private set; }
    public Vector3 LastPosition { get; private set; }
    public ProjectileConfigSO Config {get; private set;}
    protected int pierceCount;
    protected Entity lastHitTarget;

    public virtual void Init(Weapon weapon)
    {
        OwnerWeapon = weapon;
        LastPosition = transform.position;
        Config = weapon.Config.ProjectileConfig;
        pierceCount = weapon.Config.AttackPierce;
        lastHitTarget = null;
    }

    public virtual void OnMove()
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
            ProjectileManager.Instance.ToDespawn(this);
        }
    }

    public virtual void OnDespawn()
    {
        
    }
}