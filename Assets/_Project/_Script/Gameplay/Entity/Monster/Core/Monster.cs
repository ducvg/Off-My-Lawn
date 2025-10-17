using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [field: SerializeField] public MonsterConfigSO Config { get; private set; }

    public void Init()
    {
        GraphicController.Init();
        EquipmentController.Init(this);

        EquipmentController
            .WithEquipment(Config.DefaultWeapon)
            .WithEquipment(Config.DefaultShield)
            .WithEquipment(Config.DefaultArmors);
    }
}
