using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crystal", menuName = "Data Object/Currency/Crystal Config")]
public class CrystalConfigSO : ScriptableObject
{
    [field: SerializeField] public float Amount { get; private set; } = 25f;
    [field: SerializeField] public float Size { get; private set; } = 1f;
    [field: SerializeField] public Color Color { get; private set; }
}
