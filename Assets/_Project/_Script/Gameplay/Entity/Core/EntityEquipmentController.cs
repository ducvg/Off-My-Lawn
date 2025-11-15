using System;
using System.Buffers;
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
        if (Weapon)
        {
            Weapon.Unequip(ownerEntity);
            Destroy(Weapon.gameObject);
            Weapon = null;
        }
        if (!weaponConfig) return this;

        Weapon = Instantiate(weaponConfig.Prefab, EquipmentSlot[BodySlot.RightHand]);
        Weapon.Init(weaponConfig);
        Weapon.Equip(ownerEntity);

        ownerEntity.ChangeState(new EquipState());
        return this;
    }

    public EntityEquipmentController WithShield(ShieldConfigSO shieldConfig)
    {
        if (Shield)
        {
            Shield.Unequip(ownerEntity);
            Destroy(Shield.gameObject);
            Shield = null;
        }
        if (!shieldConfig) return this;

        Shield = Instantiate(shieldConfig.Prefab, EquipmentSlot[BodySlot.LeftHand]);
        Shield.SetConfig(shieldConfig);
        Shield.Equip(ownerEntity);

        ownerEntity.ChangeState(new EquipState());
        return this;
    }

    public EntityEquipmentController WithArmor(ArmorConfigSO armorConfig)
    {
        if (Armors.TryGetValue(armorConfig.EquipSlot, out var existingArmor))
        {
            existingArmor.Unequip(ownerEntity);
            Destroy(existingArmor.gameObject);
            Armors.Remove(armorConfig.EquipSlot);
        }
        if (!armorConfig) return this;

        var newArmor = Instantiate(armorConfig.Prefab, EquipmentSlot[armorConfig.EquipSlot]);
        ownerEntity.GraphicController.AddOutfitMaterial(newArmor.Material);
        newArmor.SetConfig(armorConfig);
        newArmor.Equip(ownerEntity);
        Armors[armorConfig.EquipSlot] = newArmor;

        return this;
    }
    public EntityEquipmentController WithArmor(ArmorConfigSO[] armors)
    {
        int count = armors.Length;
        if(count == 0)
        {
            ClearArmors();
            return this;   
        }
        for(int i = 0; i < count; i++)
        {
            WithArmor(armors[i]);
        }
        return this;
    }

    void ClearArmors()
    {
        int count = Armors.Values.Count;
        if(count == 0) return;
        
        Armor[] removes = ArrayPool<Armor>.Shared.Rent(count);
        Armors.Values.CopyTo(removes, 0);
        for (int i = 0; i < count; i++)
        {
            var armor = removes[i];
            armor.Unequip(ownerEntity);
            if(armor) Destroy(armor.gameObject);
        }
        Armors.Clear();
        ArrayPool<Armor>.Shared.Return(removes);
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
            ownerEntity.GraphicController.RemoveOutfitMaterial(armor.Material);
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