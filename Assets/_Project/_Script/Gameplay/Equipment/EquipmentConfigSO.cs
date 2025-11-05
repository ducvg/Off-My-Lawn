using UnityEngine;

public abstract class EquipmentConfigSO<TEquipment> : ScriptableObject where TEquipment : Equipment
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public TEquipment Prefab { get; private set; }

    [field: SerializeField] public BodySlot EquipSlot { get; private set; } = BodySlot.Head;
}

public enum BodySlot
{
    RightHand,
    LeftHand,
    Head,
}