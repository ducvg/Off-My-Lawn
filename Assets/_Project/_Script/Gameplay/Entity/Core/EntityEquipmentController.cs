using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
using ZLinq;


public class EntityEquipmentController : MonoBehaviour
{
    [field: SerializeField] public SerializedDictionary<BodySlot, Transform> EquipmentSlot { get; private set; }
    public Weapon Weapon { get; private set; } //right hand
    public Shield Shield { get; private set; } //left hand
    public Dictionary<BodySlot, Armor> Armors { get; private set; } = new(); 
    private Entity ownerEntity;

    public void Init(Entity owner)
    {
        ownerEntity = owner;
    }

    public EntityEquipmentController WithWeapon(WeaponConfigSO weaponConfig)
    {
        if (!weaponConfig) return this;
        if (Weapon)
        {
            Weapon.Unequip(ownerEntity);
            Destroy(Weapon.gameObject);
            Weapon = null;
        }

        Weapon = Instantiate(weaponConfig.Prefab, EquipmentSlot[BodySlot.RightHand]);
        Weapon.Init(weaponConfig);
        Weapon.Equip(ownerEntity);

        ownerEntity.ChangeState(new EquipState());
        return this;
    }

    public EntityEquipmentController WithShield(ShieldConfigSO shieldConfig)
    {
        if (!shieldConfig) return this;
        if (Shield)
        {
            Shield.Unequip(ownerEntity);
            Destroy(Shield.gameObject);
            Shield = null;
        }

        Shield = Instantiate(shieldConfig.Prefab, EquipmentSlot[BodySlot.LeftHand]);
        Shield.SetConfig(shieldConfig);
        Shield.Equip(ownerEntity);

        ownerEntity.ChangeState(new EquipState());
        return this;
    }

    public EntityEquipmentController WithArmor(ArmorConfigSO armorConfig)
    {
        if (!armorConfig) return this;
        if (Armors[armorConfig.EquipSlot])
        {
            Armors[armorConfig.EquipSlot].Unequip(ownerEntity);
            Destroy(Armors[armorConfig.EquipSlot].gameObject);
            Armors.Remove(armorConfig.EquipSlot);
        }

        var newArmor = Instantiate(armorConfig.Prefab, EquipmentSlot[armorConfig.EquipSlot]);
        newArmor.Equip(ownerEntity);
        Armors[armorConfig.EquipSlot] = newArmor;

        return this;
    }
    public EntityEquipmentController WithArmor(ArmorConfigSO[] armors)
    {
        foreach (var armor in armors)
        {
            WithArmor(armor);
        }
        return this;
    }

    public void UnequipWeapon(Weapon weapon)
    {
        if (Weapon == weapon)
        {
            Weapon = null;
        }
    }
    public void UnequipShield(Shield shield)
    {
        if (Shield == shield)
        {
            Shield = null;
        }
    }
    public void UnequipArmor(Armor armor)
    {
        var slot = armor.Config.EquipSlot;
        if (Armors.ContainsKey(slot))
        {
            Armors.Remove(slot);
        }
    }

#if UNITY_EDITOR
    [Button]
    private void FetchEquipmentSlots()
    {
        EquipmentSlot = new SerializedDictionary<BodySlot, Transform>
        {
            { BodySlot.RightHand, transform.parent.Descendants().FirstOrDefault(t => t.name == "handslot.r") },
            { BodySlot.LeftHand, transform.parent.Descendants().FirstOrDefault(t => t.name == "handslot.l") },
            { BodySlot.Head, transform.parent.Descendants().FirstOrDefault(t => t.name == "head") },
        };
    }
#endif
}