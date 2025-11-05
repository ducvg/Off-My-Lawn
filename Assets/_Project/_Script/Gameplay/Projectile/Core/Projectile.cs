using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IUpdate
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void OnUpdate()
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
            OnDespawn();
        }
    }

    protected virtual void OnEnable()
    {
        GameManager.Instance.TryRegisterUpdate(this);
    }
    
    protected virtual void OnDisable()
    {
#if UNITY_EDITOR
        if (!GameManager.Instance) return;
#endif
        GameManager.Instance.TryDeregisterUpdate(this);
    }

    public virtual void OnDespawn()
    {
        ProjectileManager.Instance.Release(OwnerWeapon.Config.ProjectileConfig.Prefab, this);
    }
}