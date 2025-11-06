using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Factory", menuName = "Data Object/Factory/Card Factory")]
public class CardFactory : ScriptableObject
{
    // [SerializeField] private SerializedDictionary<HeroType, Color> heroTypeColors;
    [SerializeField] private Card cardPrefab;

    public Card CreateCard(EntityConfigSO entityConfig, Transform parent)
    {
        Card card = Instantiate(cardPrefab, parent);
        card.Init(entityConfig);

        return card;
    }
}