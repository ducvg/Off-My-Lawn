using UnityEngine;

public class EquipmentConfigSO<TEquipment> : ScriptableObject where TEquipment : Equipment
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public TEquipment Prefab { get; private set; }

    [field: SerializeField] public EquipmentType Type { get; private set; } = EquipmentType.Helmet;
}

public enum EquipmentType
{
    MainHand,
    OffHand,
    Helmet,
}