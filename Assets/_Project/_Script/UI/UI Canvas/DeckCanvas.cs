using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DeckCanvas : BaseCanvas
{
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private Vector2 gameplayPosition;
    [SerializeField] private RectTransform slotContainer;
    [SerializeField] private Transform[] slots;

    public void ToGameplayPosition()
    {
        Tween.StopAll(slotContainer);
        Tween.UIAnchoredPosition(slotContainer, gameplayPosition, 0.3f, ease: Ease.Linear);
    }

    public Transform GetEmptySlot()
    {
        int length = slots.Length;
        for(int i = 0; i < length; i++)
        {
            if(slots[i].childCount == 0) return slots[i];
        }
        return null;
    }

    public void SlotInCards()
    {
        foreach(var card in CardManager.Instance.DeckCards)
        {
            var slot = GetEmptySlot();
            if(!slot) return;

            card.gameObject.SetActive(true);
            card.transform.SetParent(slot);
            card.transform.position = slot.position;
        }
    }
}