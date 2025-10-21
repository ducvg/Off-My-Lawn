using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [field: SerializeField] public HeroConfigSO Config { get; private set; }

    public override void Init()
    {
        health = Config.MaxHealth;

        GraphicController.Init(this);
        EquipmentController.Init(this);

        EquipmentController
            .WithWeapon(Config.DefaultWeapon)
            .WithShield(Config.DefaultShield)
            .WithArmor(Config.DefaultArmors);
    }
}
