using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#pragma warning disable CS8524 //suppress warning switch not have default case

public class EquipmentController : MonoBehaviour
{
    [SerializeField] private Transform WeaponSlot, ShieldSlot;
    [SerializeField] private Transform ArmorHeadSlot, ArmorChestSlot, ArmorArmSlot, ArmorLegSlot;

    public Entity OwnerEntity { get; private set; }
    public Weapon Weapon { get; private set; }
    public Shield Shield { get; private set; }
    public Dictionary<Transform, List<Armor>> Armors { get; private set; } = new();

    public void Init(Entity owner)
    {
        OwnerEntity = owner;
        Armors[ArmorHeadSlot] = new List<Armor>();
        Armors[ArmorChestSlot] = new List<Armor>();
        Armors[ArmorArmSlot] = new List<Armor>();
        Armors[ArmorLegSlot] = new List<Armor>();
    }

    public EquipmentController WithEquipment(EquipmentConfigSO equipmentConfig)
    {
        Instantiate(equipmentConfig.Prefab).Equip(OwnerEntity);
        return this;
    }

    public EquipmentController WithEquipment(EquipmentConfigSO[] equipmentConfigs)
    {
        var length = equipmentConfigs.Length;
        for(int i = 0; i < length; ++i)
        {
            Instantiate(equipmentConfigs[i].Prefab).Equip(OwnerEntity);
        }
        return this;
    }
}