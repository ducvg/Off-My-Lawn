using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data Object/Equipment/Weapon Config")]
public class WeaponConfigSO : EquipmentConfigSO<Weapon>
{
    [field: Header("Actions")]
    [field: SerializeField] public AnimationClip AttackAnimation { get; private set; }
    
    [field: SerializeField] public ParticleSystem HitParticle { get; private set; }
    [field: SerializeReference] public IAttackEffect[] AttackEffects { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Cooldown { get; private set; } = 1f;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
}