using Cysharp.Text;
using TMPro;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private TextMeshProUGUI moneyText;

    public void AddCard(HeroConfigSO heroConfig)
    {
        cardFactory.CreateCard(heroConfig, cardParent);
    }

    public void SetMoneyText(float amount)
    {
        moneyText.SetTextFormat("{0}", amount);
    }
}
