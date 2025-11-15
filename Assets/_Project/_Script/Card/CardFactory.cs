using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Factory", menuName = "Data Object/Factory/Card Factory")]
public class CardFactory : ScriptableObject
{
    [SerializeField] private Card cardPrefab;

    public Card CreateCard(EntityConfigSO entityConfig, Transform parent)
    {
        Card card = Instantiate(cardPrefab, parent);
        card.Init(entityConfig);

        return card;
    }

    public Card CreateCard(EntityConfigSO entityConfig, Vector3 position, Transform parent)
    {
        Card card = Instantiate(cardPrefab, position, Quaternion.identity, parent);
        card.Init(entityConfig);

        return card;
    }

    public Card CreateCard(EntityConfigSO entityConfig)
    {
        Card card = Instantiate(cardPrefab);
        card.Init(entityConfig);

        return card;
    }
}