using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public GraphicController GraphicController { get; private set; }
    [field: SerializeField] public EquipmentController EquipmentController { get; private set; }
    [SerializeField] protected float health;
    //states

    public abstract void Init();
}