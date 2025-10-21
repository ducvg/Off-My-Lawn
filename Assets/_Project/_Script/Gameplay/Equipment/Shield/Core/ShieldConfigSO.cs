using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Data Object/Equipment/Shield Config")]
public class ShieldConfigSO : EquipmentConfigSO<Shield>
{
    [field: SerializeField] public float BaseHealth { get; private set; }
}
