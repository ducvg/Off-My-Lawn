using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private CardFactory cardFactory;
    public HashSet<Card> DeckCards { get; private set; } = new();

    public void SpawnDefaultCards(List<EntityID> entiyIds)
    {
        ClearCards();
        foreach(var id in entiyIds)
        {
            var config = GameDatabase.Instance.EntityDictionary[id];
            var card = cardFactory.CreateCard(config);
            card.SetSelectable(false);
            DeckCards.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    public void AddCardToDeck(Card card)
    {
        var slot = UIManager.Instance.GetCanvas<DeckCanvas>().GetEmptySlot();
        if(!slot) return;

        DeckCards.Add(card);
        card.transform.SetParent(slot);
        Tween.StopAll(card.transform);
        Tween.Position(card.transform, slot.position, 0.2f, ease: Ease.Linear);  
    }

    public void RemoveCard(Card card)
    {
        DeckCards.Remove(card);
    }

    public void SetDeckActive(bool isActive)
    {
        foreach(var card in DeckCards)
        {
            if(card) card.SetSelectable(isActive);
        }
    }

    public bool IsCardInDeck(Card card)
    {
        return DeckCards.Contains(card);
    }

    public void ClearCards()
    {
        foreach(var card in DeckCards)
        {
            Destroy(card.gameObject);
        }
        DeckCards.Clear();
    }

}
