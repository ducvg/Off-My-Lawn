using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private CardFactory cardFactory;
    public HashSet<Card> Cards { get; private set; } = new();

    public void SpawnDefaultCards(List<EntityID> entiyIds)
    {
        ClearCards();
        foreach(var id in entiyIds)
        {
            var config = EntityFactory.Instance.GetEntityConfig(id);
            var card = cardFactory.CreateCard(config);
            card.SetSelectable(false);
            Cards.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    public void AddCard(Card card)
    {
        var slot = UIManager.Instance.GetCanvas<DeckCanvas>().GetEmptySlot();
        if(!slot) return;

        Cards.Add(card);
        card.transform.SetParent(slot);
        Tween.StopAll(card.transform);
        Tween.Position(card.transform, slot.position, 0.2f, ease: Ease.Linear);  
    }

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
    }

    public void SetCardActive(bool isActive)
    {
        foreach(var card in Cards)
        {
            if(card) card.SetSelectable(isActive);
        }
    }

    public bool IsCardInDeck(Card card)
    {
        return Cards.Contains(card);
    }

    public void ClearCards()
    {
        foreach(var card in Cards)
        {
            Destroy(card.gameObject);
        }
        Cards.Clear();
    }

}
