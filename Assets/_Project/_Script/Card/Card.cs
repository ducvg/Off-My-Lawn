using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;

    [field: SerializeField] public HeroConfigSO Config { get; private set; }

    public void Init(in CardData cardData)
    {
        Config = cardData.Config;

        icon.sprite = cardData.Config.Icon;
        costText.SetTextFormat("{0}", cardData.Config.Cost);
        background.color = cardData.BackgroundColor;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        InputManager.Instance.OnBeginDragCard(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        InputManager.Instance.OnDragCard();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InputManager.Instance.OnEndDragCard();
    }

    
}