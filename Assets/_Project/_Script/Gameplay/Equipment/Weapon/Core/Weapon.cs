using System;
using System.Collections;
using PrimeTween;
using UnityEngine;

public abstract class Weapon : Equipment
{
    public WeaponConfigSO Config { get; private set; }
    public LayerMask targetLayerMask { get; protected set; }
    protected RaycastHit[] raycastHitBuffer = new RaycastHit[1];
    protected Ray ray;
    protected Entity ownerEntity;
    protected float lastAttackTime;
    protected Tween attackTween;

    public virtual void Init(WeaponConfigSO config)
    {
        Config = config;
        // raycastHitBuffer = new RaycastHit[Config.TargetCount];
    }

    public override void Equip(Entity entity)
    {
        ownerEntity = entity;

        entity.GraphicController
            .WithOverrideAnimation(Animation.EQUIP, Config.EquipAnimation)
            .WithOverrideAnimation(Animation.IDLE, Config.IdleAnimation)
            .WithOverrideAnimation(Animation.ATTACK, Config.AttackAnimation)
            .ApplyAnimatorOverrides();

        transform.localRotation = ownerEntity.transform.localRotation * Quaternion.Euler(0f, 180f, 0f);
        targetLayerMask = GetTargetLayerMask(entity);
        lastAttackTime = -Config.AttackCooldown; 
    }

    public virtual void Attack(Entity attacker)
    {
        lastAttackTime = Time.time;
        attackTween = Tween.Delay(Config.AttackDelay / Config.AttackSpeed)
                        .OnComplete(this, target => target.ExecuteAttack());
    }

    protected abstract void ExecuteAttack();

    public virtual void CancelAttack()
    {
        attackTween.Stop();
    }

    public override void Unequip(Entity entity)
    {
        CancelAttack();
    }

    public virtual bool HasTargetInRange()
    {
        ray = new Ray(ownerEntity.AttackPoint.position, ownerEntity.transform.forward);
        int hitsCount = Physics.RaycastNonAlloc(ray, raycastHitBuffer, Config.AttackRange, targetLayerMask);
        if (hitsCount <= 0) return false;
        if (raycastHitBuffer[0].point.x > GameConstant.GRID_BOUND_X_MAX)
        {
            return false;
        }
        return true;
    }

    public virtual bool IsOnCooldown()
    {
        return Time.time - lastAttackTime < Config.AttackCooldown;
    }

    public LayerMask GetTargetLayerMask(Entity attacker)
    {
        if (attacker is Hero)
        {
            return LayerMask.GetMask("Monster");
        }
        else // if (attacker is Monster)
        {
            return LayerMask.GetMask("Hero");
        }
    }

    void OnDestroy()
    {
        CancelAttack();        
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying || !ownerEntity) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ownerEntity.AttackPoint.position, ownerEntity.transform.forward * Config.AttackRange);
    }
#endif
}