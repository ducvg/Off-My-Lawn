using System;
using UnityEngine;

[Serializable]
public class Upgrade
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeReference, Subclass] public IUpgradeStrategy[] UpgradeStrategies { get; private set; }
}
