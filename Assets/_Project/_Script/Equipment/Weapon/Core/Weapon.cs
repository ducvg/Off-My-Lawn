using System;
using System.Collections;
using System.Runtime.CompilerServices;
using PrimeTween;
using UnityEngine;

public abstract class Weapon : Equipment
{
    public WeaponConfigSO Config { get; private set; }
    public LayerMask TargetLayerMask { get; protected set; }
    public Entity OwnerEntity { get; protected set; }
    protected RaycastHit[] raycastHitBuffer = new RaycastHit[1];
    protected Ray ray;
    protected float lastAttackTime, attackLength;
    protected Tween attackTween;

    public virtual void Init(WeaponConfigSO config)
    {
        Config = config;
        // raycastHitBuffer = new RaycastHit[Config.TargetCount];
    }

    public override void Equip(Entity entity)
    {
        OwnerEntity = entity;

        entity.GraphicController
            .WithOverrideAnimation(Animation.EQUIP, Config.EquipAnimation)
            .WithOverrideAnimation(Animation.IDLE, Config.IdleAnimation)
            .WithOverrideAnimation(Animation.ATTACK, Config.AttackAnimation)
            .ApplyAnimatorOverrides();

        transform.forward = OwnerEntity.transform.forward;
        TargetLayerMask = GetTargetLayerMask(entity);
        lastAttackTime = -Config.AttackCooldown; 
        attackLength = Config.AttackAnimation.length; 
    }

    public virtual void Attack(Entity attacker)
    {
        lastAttackTime = Time.time;
        attackTween = Tween.Delay(Config.AttackDelay / OwnerEntity.StatModifier.GetFinalAttackSpeed())
                        .OnComplete(this, target => target.ExecuteAttack());
    }

    protected abstract void ExecuteAttack();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual bool HasTarget()
    {
        ray = new Ray(OwnerEntity.AttackPoint.position, OwnerEntity.transform.forward);
        int hitsCount = Physics.RaycastNonAlloc(ray, raycastHitBuffer, Config.AttackRange, TargetLayerMask);
        if (hitsCount <= 0) return false;
        if (raycastHitBuffer[0].point.x > GameConstant.GRID_BOUND_X_MAX)
        {
            return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual bool IsOnCooldown()
    {
        return Time.time - lastAttackTime < Config.AttackCooldown;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual bool IsAttackFinished()
    {
        return Time.time - lastAttackTime > attackLength / OwnerEntity.StatModifier.GetFinalAttackSpeed();
    }

    public virtual void CancelAttack()
    {
        attackTween.Stop();
    }

    public override void Unequip(Entity entity)
    {
        CancelAttack();
    }


    public LayerMask GetTargetLayerMask(Entity attacker)
    {
        if (attacker is Hero)
        {
            return LayerMask.GetMask("Monster");
        }
        else
        {
            return LayerMask.GetMask("Hero");
        }
    }

    void OnDestroy()
    {
        CancelAttack();        
    }

#if UNITY_EDITOR
    // protected virtual void OnDrawGizmos()
    // {
    //     if (!Application.isPlaying || !ownerEntity) return;
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(ray);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(ownerEntity.AttackPoint.position, ownerEntity.transform.forward * Config.AttackRange);
    // }
#endif
}