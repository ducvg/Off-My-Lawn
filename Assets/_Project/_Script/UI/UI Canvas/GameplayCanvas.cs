using System.Collections.Generic;
using Cysharp.Text;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private RectTransform FlagPrefab;
    [SerializeField] private FloatValueSO crystalValue;
    [SerializeField] private FloatValueSO levelProgressValue;
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private RectTransform progressIndicator;
    [SerializeField] private RectTransform progressBarParent;
    [SerializeField] private Image progressBarFill;
    private List<RectTransform> flags = new();

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

    public void SetCrystalText(float amount)
    {
        crystalText.SetTextFormat("{0}", amount);
    }

    public void ClearFlags()
    {
        foreach (var flag in flags)
        {
            Destroy(flag.gameObject);
        }
        flags.Clear();
    }

    public void AddProgressFlag(float lerpFactor)
    {
        float xPos = Mathf.Lerp(0, -progressBarParent.rect.width, lerpFactor);
        RectTransform flag = Instantiate(FlagPrefab, progressBarParent);
        flag.anchoredPosition = new Vector2(xPos, 0);
        flags.Add(flag);
    }

    public void UpdateProgressBar(float lerpFactor)
    {
        progressBarFill.fillAmount = lerpFactor;
        float xPos = Mathf.Lerp(0, -progressBarParent.rect.width, lerpFactor);
        progressIndicator.anchoredPosition = new Vector2(xPos, progressIndicator.anchoredPosition.y);
    }

    void OnEnable()
    {
        levelProgressValue.OnValueChanged += UpdateProgressBar;
        crystalValue.OnValueChanged += SetCrystalText;
    }
    void OnDisable()
    {
        levelProgressValue.OnValueChanged -= UpdateProgressBar;
        crystalValue.OnValueChanged -= SetCrystalText;
    }
}
