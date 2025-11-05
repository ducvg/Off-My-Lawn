using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Data Object/Equipment/Projectile Config")]
public class ProjectileConfigSO : ScriptableObject
{
    [field: SerializeField] public Projectile Prefab { get; private set; }
    [field: SerializeField] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public bool UseCurve { get; private set; } = false;
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; } = AnimationCurve.Linear(0, 1, 1, 1);
}