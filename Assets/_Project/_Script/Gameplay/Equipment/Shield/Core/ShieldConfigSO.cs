using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Data Object/Equipment/Shield Config")]
public class ShieldConfigSO : EquipmentConfigSO<Shield>
{
    [field: Header("Animations")]
    [field: SerializeField] public AnimationClip EquipAnimation { get; private set; }
    [field: SerializeField] public AnimationClip IdleAnimation { get; private set; }
    [field: SerializeField] public AnimationClip HurtAnimation { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float BaseHealth { get; private set; }
}
