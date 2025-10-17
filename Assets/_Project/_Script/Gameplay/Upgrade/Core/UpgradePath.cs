using System;
using UnityEngine;

[Serializable]
public class UpgradePath
{
    [field: SerializeField] public Upgrade[] Upgrades { get; private set; }
}
