using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour, IUpdate
{
    public static readonly Color HurtEmission = new Color(0.2f, 0.2f, 0.2f, 1f);
    [field: SerializeField] public EntityGraphicController GraphicController { get; private set; }
    [field: SerializeField] public EntityEquipmentController EquipmentController { get; private set; }
    [field: SerializeField] public Transform AttackPoint { get; private set; }
    [SerializeField] protected Collider hitBox;
    public EntityConfigSO Config { get; private set; }
    public StatModifier StatModifier { get; private set; } = new();
    protected IState currentState;
    protected Dictionary<Type, IStatusEffect> activeStatusEffects = new();
    protected event Action<Entity> statusUpdateAction;

    protected float health;

    #region Setups
    protected virtual void Awake()
    {
        SetColliderActive(false);
    }

    public virtual void Init(EntityConfigSO config)
    {
        SetColliderActive(true);
        LevelManager.Instance.RegisterEntityCollider(hitBox, this);
        Config = config;
        ResetStatusEffects();

        StatModifier.Init(this);
        health = Config.BaseHealth;

        GraphicController.Init(this);
        SetupGraphics();
        EquipmentController.Init(this);
        SetupEquipment();
    }

    protected virtual void SetupGraphics()
    {
        GraphicController
            .WithOverrideAnimation(Animation.MOVE, Config.MoveAnimation[Random.Range(0, Config.MoveAnimation.Length)])
            .WithOverrideAnimation(Animation.DIE, Config.DieAnimation)
            .ApplyAnimatorOverrides();
    }

    protected virtual void SetupEquipment()
    {
        EquipmentController
            .WithWeapon(Config.DefaultWeaponConfig)
            .WithShield(Config.DefaultShieldConfig)
            .WithArmor(Config.DefaultArmorConfigs);
    }
    #endregion

    public virtual void OnUpdate()
    {
        statusUpdateAction?.Invoke(this);
        if (currentState != null)
        {
            currentState.OnUpdate(this);
        }
    }

    public virtual void SyncAnimationSpeed()
    {
        GraphicController.Animator.SetFloat(Animation.MoveSpeedHash, StatModifier.GetFinalMoveSpeed());
        GraphicController.Animator.SetFloat(Animation.AttackSpeedHash, StatModifier.GetFinalAttackSpeed());
    }

#region Status Effects
    private void ResetStatusEffects()
    {
        foreach (var statusEffect in activeStatusEffects.Values)
        {
            statusEffect.OnRemove(this);
        }
        activeStatusEffects.Clear();
    }
    public virtual void ApplyStatusEffect(IStatusEffect statusEffect)
    {
        Type type = statusEffect.GetType();
        if(activeStatusEffects.TryGetValue(type, out var existingEffect))
        {
            existingEffect.OnDuplicate(this);
            return;
        }
        if(!statusEffect.OnApply(this)) return;
        activeStatusEffects[statusEffect.GetType()] = statusEffect;
        statusUpdateAction += statusEffect.OnUpdate;
    }
    public virtual void RemoveStatusEffect(IStatusEffect statusEffect)
    {
        if(activeStatusEffects.Remove(statusEffect.GetType()))
        {
            statusEffect.OnRemove(this);
            statusUpdateAction -= statusEffect.OnUpdate;
        }
    }
#endregion

#region Death & despawn

    //shield -> armors -> health
    public virtual void TakeDamage(float damage, float damageForce = 3f, Action OnKill = null)
    {
        var shield = EquipmentController.Shield;
        if (shield)
        {
            shield.Block(this, ref damage);
        }

        foreach (var slot in EquipmentController.EquipmentSlot.Keys)
        {
            if (EquipmentController.Armors.TryGetValue(slot, out var armor))
            {
                armor.Block(this, ref damage);
            }
        }
        
        GraphicController.BlinkEmissionAll(HurtEmission, 0.15f); 
        health -= damage;
        if (health <= 0)
        {
            OnKill?.Invoke();
            OnDie();
        }
    }

    protected virtual void OnDie()
    {
        ChangeState(new DieState());
    }

    public virtual void Despawn()
    {
    }

    public virtual bool IsDead()
    {
        return health <= 0;
    }
    #endregion

    public virtual void ChangeState(in IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual void SetColliderActive(bool enabled)
    {
        hitBox.enabled = enabled;
    }
    protected virtual void OnEnable()
    {
        GameManager.Instance.TryRegisterUpdate(this);
    }
    protected virtual void OnDisable()
    {
        if(!GameManager.Instance) return; //Scene random destroy
        GameManager.Instance.TryUnregisterUpdate(this);
    }
    private void OnDestroy()
    {
        if (!LevelManager.Instance) return;
        LevelManager.Instance.UnregisterEntityCollider(hitBox);
    }
}
