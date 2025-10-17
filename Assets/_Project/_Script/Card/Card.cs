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

    [field: SerializeField] public HeroConfigSO HeroConfig { get;  private set; }

    public void Init(in CardData cardData)
    {
        HeroConfig = cardData.HeroConfig;
        icon.sprite = cardData.HeroConfig.Icon;
        costText.SetTextFormat("{0}", cardData.HeroConfig.Cost);
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