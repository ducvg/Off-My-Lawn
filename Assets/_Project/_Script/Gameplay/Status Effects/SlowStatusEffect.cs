using System;
using UnityEngine;

[Serializable]
public class SlowStatusEffect : IStatusEffect
{
    [SerializeField] float addPercent;
    [SerializeField] float duration;
    [SerializeField, ColorUsage(showAlpha: false, hdr: true)] Color emissionColor;
    private float elapsedTime;

    public SlowStatusEffect(float addPercent, float duration, Color emissionColor)
    {
        this.addPercent = addPercent;
        this.duration = duration;
        this.emissionColor = emissionColor;
        elapsedTime = 0f;
    }

    public void OnDuplicate(Entity target)
    {
        elapsedTime = 0f;
    }

    public void OnApply(Entity target)
    {
        target.StatBonus.MoveSpeedMultiplier += addPercent;
        target.StatBonus.AttackSpeedMultiplier += addPercent;
        target.GraphicController.SetEmissionAll(emissionColor);
        target.SyncAnimationSpeed();
        elapsedTime = 0f;
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
        target.StatBonus.MoveSpeedMultiplier -= addPercent;
        target.StatBonus.AttackSpeedMultiplier -= addPercent;
        target.GraphicController.SetEmissionAll(Color.black);
        target.SyncAnimationSpeed();
    }

    public IStatusEffect Clone()
    {
        return new SlowStatusEffect(addPercent, duration, emissionColor);
    }
}