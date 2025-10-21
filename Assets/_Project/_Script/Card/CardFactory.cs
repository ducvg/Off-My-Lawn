using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Factory", menuName = "Data Object/Factory/Card Factory")]
public class CardFactory : ScriptableObject
{
    [SerializeField] private SerializedDictionary<HeroType, Color> heroTypeColors;
    [SerializeField] private Card cardPrefab;

    public Card CreateCard(HeroConfigSO heroConfig, Transform parent)
    {
        Card card = Instantiate(cardPrefab, parent);
        card.Init(new CardData()
        {
            Config = heroConfig,
            BackgroundColor = heroTypeColors[heroConfig.HeroType],
        });

        return card;
    }
}

public struct CardData
{
    public HeroConfigSO Config;
    public Color BackgroundColor;
}
