using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;

    public EntityConfigSO EntityConfig { get; private set; }

    public void Init(EntityConfigSO config)
    {
        EntityConfig = config;

        icon.sprite = config.Icon;
        costText.SetTextFormat("{0}", config.CrystalCost);
        background.color = config.CardColor;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}