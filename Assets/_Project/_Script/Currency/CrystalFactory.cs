using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFactory : Singleton<CrystalFactory>
{
    [SerializeField] private Crystal crystalPrefab;
    [SerializeField] private CrystalConfigSO normalCrystalConfig; //small, large,...
    private UnityPoolFactory<Crystal> crystalFactory = new();

    void Start()
    {
        crystalFactory.Preload(crystalPrefab, 10);
    }

    public Crystal SpawnNormal(Vector3 position)
    {
        var crystal = crystalFactory.Spawn(crystalPrefab, position);
        crystal.Init(normalCrystalConfig);
        return crystal;
    }

    public void Release(Crystal crystal)
    {
        crystalFactory.Release(crystalPrefab, crystal);
    }
}
