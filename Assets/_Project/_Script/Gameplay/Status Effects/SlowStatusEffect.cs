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

    public bool OnApply(Entity target)
    {
        target.StatModifier.MoveSpeedModifier.bonusAdd += addPercent;
        target.StatModifier.AttackSpeedModifier.bonusAdd += addPercent;
        target.GraphicController.SetEmissionAll(emissionColor);
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
        target.StatModifier.MoveSpeedModifier.bonusAdd -= addPercent;
        target.StatModifier.AttackSpeedModifier.bonusAdd -= addPercent;
        target.GraphicController.SetEmissionAll(Color.black);
    }

    public IStatusEffect Clone()
    {
        return new SlowStatusEffect(addPercent, duration, emissionColor);
    }
}
