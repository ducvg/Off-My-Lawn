using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data Object/Equipment/Weapon Config")]
public class WeaponConfigSO : EquipmentConfigSO<Weapon>
{
    [field: Header("Animations")]
    [field: SerializeField] public AnimationClip EquipAnimation { get; private set; }
    [field: SerializeField] public AnimationClip IdleAnimation { get; private set; }
    [field: SerializeField] public AnimationClip AttackAnimation { get; private set; }

    [field: Header("Actions")]
    [field: SerializeField] public ParticleSystem HitParticle { get; private set; }
    [field: SerializeReference] public IAttackEffect[] AttackEffects { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float AttackRange { get; private set; } = 1f;
    [field: SerializeField, LabelText("Attack Cooldown (s)")] public float AttackCooldown { get; private set; } = 1f;
    [field: SerializeField, LabelText("Attack Delay (s)")] public float AttackDelay { get; private set; } = 0.1f;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
    [field: SerializeField] public int AttackPierce { get; private set; } = 1;

    [field: Header("Projectile")]
    [field: SerializeField] public ProjectileConfigSO ProjectileConfig { get; private set; }
}