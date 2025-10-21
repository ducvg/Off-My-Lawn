using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
public class EquipmentController : MonoBehaviour
{
    [field: SerializeField] public SerializedDictionary<EquipmentType, Transform> EquipmentSlot { get; private set; }
    private Weapon weapon;
    private Shield shield;
    private Dictionary<EquipmentType, Armor> armors = new();
    private Entity ownerEntity;

    public void Init(Entity owner)
    {
        ownerEntity = owner;
    }

    public EquipmentController WithWeapon(WeaponConfigSO weaponConfig)
    {
        if(!weaponConfig) return this;
        if(weapon) weapon.Unequip();
        var newWeapon = Instantiate(weaponConfig.Prefab, EquipmentSlot[weaponConfig.Type]);
        newWeapon.Equip(ownerEntity);
        weapon = newWeapon;
        return this;
    }

    public EquipmentController WithShield(ShieldConfigSO shieldConfig)
    {
        if(!shieldConfig) return this;
        if(shield) shield.Unequip();
        var newShield = Instantiate(shieldConfig.Prefab, EquipmentSlot[shieldConfig.Type]);
        newShield.Equip(ownerEntity);
        shield = newShield;
        return this;
    }

    public EquipmentController WithArmor(ArmorConfigSO armorConfig)
    {
        if(!armorConfig) return this;
        if(armors[armorConfig.Type]) armors[armorConfig.Type].Unequip();
        var newArmor = Instantiate(armorConfig.Prefab, EquipmentSlot[armorConfig.Type]);
        newArmor.Equip(ownerEntity);
        armors[armorConfig.Type] = newArmor;
        return this;
    }
    public EquipmentController WithArmor(ArmorConfigSO[] armorConfigs)
    {
        foreach(var armorConfig in armorConfigs)
        {
            WithArmor(armorConfig);
        }
        return this;
    }
}