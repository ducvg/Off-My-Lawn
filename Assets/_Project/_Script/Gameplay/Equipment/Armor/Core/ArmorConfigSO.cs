using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Data Object/Equipment/Armor Config")]
public class ArmorConfigSO : EquipmentConfigSO
{
    [field: SerializeField] public float BaseHealth { get; private set; }
}