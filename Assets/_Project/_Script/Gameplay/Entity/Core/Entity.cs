using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour, IUpdate
{
    public BonusStats StatBonus { get; private set; } = new();
    public EntityConfigSO Config { get; private set; }
    [field: SerializeField] public EntityGraphicController GraphicController { get; private set; }
    [field: SerializeField] public EntityEquipmentController EquipmentController { get; private set; }
    [field: SerializeField] public Transform AttackPoint { get; private set; }
    [SerializeField] protected Collider hitBox;
    protected float health;
    protected IState currentState;
    protected Dictionary<Type, IStatusEffect> activeStatusEffects = new();  //structs
    protected Action<Entity> statusUpdates;
    public static readonly Color HurtEmission = new Color(0.2f, 0.2f, 0.2f, 1f); //nigger

    public virtual void OnCellPlaced(GameCell cell) { }

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

        StatBonus.Init(this);
        health = Config.MaxHealth;

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
        statusUpdates?.Invoke(this);
        if (currentState != null)
        {
            currentState.OnUpdate(this);
        }
    }

    public virtual void SyncAnimationSpeed()
    {
        GraphicController.Animator.SetFloat(Animation.MoveSpeedHash, StatBonus.GetFinalMoveSpeed());
        GraphicController.Animator.SetFloat(Animation.AttackSpeedHash, StatBonus.GetFinalAttackSpeed());
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
        if (activeStatusEffects.TryGetValue(type, out var existingEffect))
        {
            existingEffect.OnDuplicate(this);
            return;
        }
        activeStatusEffects[statusEffect.GetType()] = statusEffect;
        statusEffect.OnApply(this);
        statusUpdates += statusEffect.OnUpdate;
    }
    public virtual void RemoveStatusEffect(IStatusEffect statusEffect)
    {
        if (activeStatusEffects.Remove(statusEffect.GetType()))
        {
            statusEffect.OnRemove(this);
            statusUpdates -= statusEffect.OnUpdate;
        }
    }
    #endregion

    public virtual void Upgrade(int pathIndex) { }

    #region Death & despawn

    //shield -> armors -> health
    public virtual void TakeDamage(float damage, float damageForce = 3f, Action OnKill = null)
    {
        var shield = EquipmentController.Shield;
        if (shield)
        {
            shield.Block(this, ref damage);
            if (damage <= 0) return;
        }

        GraphicController.BlinkEmissionAll(HurtEmission, 0.15f); //no shield
        foreach (var armor in EquipmentController.Armors.Values)
        {
            if (armor)
            {
                armor.Block(this, ref damage);
                if (damage <= 0) return;
            }
        }

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
        LevelManager.Instance.UnregisterEntityCollider(hitBox);
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
        if (!GameManager.Instance) return; //Scene random destroy
        GameManager.Instance.TryDeregisterUpdate(this);
    }
    private void OnDestroy()
    {
        if (!LevelManager.Instance) return;
        LevelManager.Instance.UnregisterEntityCollider(hitBox);
    }
}
