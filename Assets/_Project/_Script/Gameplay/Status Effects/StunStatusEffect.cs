using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StunStatusEffect : IStatusEffect
{
    [SerializeField, Range(0f, 1f)] private float chance;
    [SerializeField] private float duration;
    private float elapsedTime;

    public StunStatusEffect(float chance, float duration)
    {
        this.chance = chance;
        this.duration = duration;
        elapsedTime = 0f;
    }

    public void OnDuplicate(Entity target)
    {
        if (Random.Range(0f, 1f) > chance) return;
        elapsedTime = 0f;
    }

    public bool OnApply(Entity target)
    {
        if (Random.Range(0f, 1f) > chance) return false;
        if (target.IsDead()) return false;
        
        target.ChangeState(new StunState());
        elapsedTime = 0f;
        return true;
    }

    public void OnUpdate(Entity target)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= duration)
        {
            target.RemoveStatusEffect(this);
        }
    }

    public void OnRemove(Entity target)
    {
        target.ChangeState(new IdleState());
    }

    public IStatusEffect Clone()
    {
        return new StunStatusEffect(chance,duration);
    }
}