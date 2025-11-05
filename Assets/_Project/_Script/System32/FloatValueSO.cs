using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Value", menuName = "Data Object/Float Value")]
public class FloatValueSO : ScriptableObject
{
    [SerializeField] private float value;
    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }
    }
    public Action<float> OnValueChanged;
    
}
