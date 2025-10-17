using UnityEngine;

public class EntityConfigSO<TEntity> : ScriptableObject where TEntity : Entity
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public TEntity Prefab { get; private set; }
}