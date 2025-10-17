using UnityEngine;

public class EquipmentConfigSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Equipment Prefab { get; private set; }
}