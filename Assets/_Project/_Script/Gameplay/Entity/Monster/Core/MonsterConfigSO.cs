using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Data Object/Entity/Monster Config")]
public class MonsterConfigSO : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Monster Prefab { get; private set; }
    [field: SerializeField] public AnimationClip MoveAnimation { get; private set; }
    [field: SerializeField] public AnimationClip DieAnimation { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public Weapon DefaultWeapon { get; private set; }
    [field: SerializeField] public Shield DefaultShield { get; private set; }
    [field: SerializeField] public Armor[] DefaultArmors { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public float BaseSpeed { get; private set; } = 1;
    [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }

}