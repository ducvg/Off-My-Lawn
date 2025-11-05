using Cysharp.Text;
using PrimeTween;
using TMPro;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private FloatValueSO crystalValue;
    Sequence crystalWarnSequence;

    public override void Setup()
    {
        base.Setup();
        SetCrystalText(crystalValue.Value);
    }

    public void WarnInsufficientCrystal()
    {
        crystalWarnSequence.Complete();
        crystalWarnSequence = Sequence.Create();
        crystalWarnSequence.Group(Tween.Color(crystalText, Color.red, 0.1f, cycleMode: CycleMode.Yoyo, cycles: 4));
        crystalWarnSequence.Group(Tween.ShakeLocalRotation(crystalText.transform, new Vector3(0, 0, 10), 0.4f));
        crystalWarnSequence.Group(Tween.Scale(crystalText.transform, Vector3.one * 1.2f, 0.1f, cycleMode: CycleMode.Yoyo, cycles: 4));
    }

    public void AddCard(EntityConfigSO entityConfig)
    {
        cardFactory.CreateCard(entityConfig, cardParent);
    }

    public void SetCrystalText(float amount)
    {
        crystalText.SetTextFormat("{0}", amount);
    }

    void OnEnable()
    {
        crystalValue.OnValueChanged += SetCrystalText;
    }
    void OnDisable()
    {
        crystalValue.OnValueChanged -= SetCrystalText;
    }
}
