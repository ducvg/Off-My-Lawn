using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shield : Equipment
{
    protected float health;

    public abstract void TakeDamage(float damage);
}
