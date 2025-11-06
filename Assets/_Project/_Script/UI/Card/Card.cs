using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IPointerEnterHandler, IPointerExitHandler, IUpdate
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image cooldownOverlay;
    public EntityConfigSO EntityConfig { get; private set; }
    float cooldownTimer;
    bool canSelect;

    public void Init(EntityConfigSO config)
    {
        EntityConfig = config;
        EndCooldown();

        icon.sprite = config.Icon;
        costText.SetTextFormat("{0}", config.CrystalCost);
        background.color = config.CardColor;
    }

    public void OnUpdate()
    {
        cooldownTimer += Time.deltaTime;
        if(cooldownTimer >= EntityConfig.CardCooldown)
        {
            EndCooldown();
            return;
        }
        cooldownOverlay.fillAmount = 1f - (cooldownTimer / EntityConfig.CardCooldown);
    }

    public void StartCooldown()
    {
        cooldownTimer = 0f;
        canSelect = false;
        cooldownOverlay.enabled = true;
        cooldownOverlay.fillAmount = 1f;
        GameManager.Instance.TryRegisterUpdate(this);
    }
    
    void EndCooldown()
    {
        canSelect = true;
        cooldownOverlay.enabled = false;
        GameManager.Instance.TryUnregisterUpdate(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canSelect) return;
        InputManager.Instance.OnBeginDragCard(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canSelect) return;
        InputManager.Instance.OnDragCard();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InputManager.Instance.OnEndDragCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canSelect) return;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canSelect) return;
        transform.localScale = Vector3.one;
    }

    void OnDestroy()
    {
        if(!GameManager.Instance) return;
        GameManager.Instance.TryUnregisterUpdate(this);
    }
}