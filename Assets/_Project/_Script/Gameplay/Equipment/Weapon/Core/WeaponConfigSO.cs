using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data Object/Equipment/Weapon Config")]
public class WeaponConfigSO : EquipmentConfigSO
{
    [field: SerializeField] public AnimationClip AttackAnimation { get; private set; }
    
    [field: SerializeField] public ParticleSystem HitParticle { get; private set; }
    [field: SerializeReference, Subclass] public IHitEffect[] HitEffects { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; } //anim speed
}