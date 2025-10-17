using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Data Object/Entity/Monster Config")]
public class MonsterConfigSO : EntityConfigSO<Monster>
{
    [field: Header("Spawn Data")]
    [field: SerializeField] public float WavePoints { get; private set; }
    [field: SerializeField, Range(0, 1)] public float WaveWeight { get; private set; }
    
    [field: Header("Stats")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int MoveSpeed { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public WeaponConfigSO DefaultWeapon { get; private set; }
    [field: SerializeField] public ShieldConfigSO DefaultShield { get; private set; }
    [field: SerializeField] public ArmorConfigSO[] DefaultArmors { get; private set; }
}
