using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
                    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image cooldownOverlay;
    public new Transform transform => _transform ??= base.transform;
    private Transform _transform;
    public EntityConfigSO EntityConfig { get; private set; }
    float cooldownTimer;
    bool isOnCooldown, isSelectable;

    public void Init(EntityConfigSO config)
    {
        EntityConfig = config;
        icon.sprite = config.Icon;
        costText.SetTextFormat("{0}", config.CrystalCost);
        background.color = config.CardColor;
        
        SetEndCooldown();
    }

    void Update()
    {
        if(GameManager.GameState != GameState.Playing) return;

        CooldownUpdate();
    }

    public void CooldownUpdate()
    {
        if (!isOnCooldown) return;

        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= EntityConfig.CardCooldown)
        {
            SetEndCooldown();
            return;
        }
        cooldownOverlay.fillAmount = 1f - (cooldownTimer / EntityConfig.CardCooldown);
    }

    public void SetOnCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = 0f;
        SetSelectable(false);
    }
    
    public void SetEndCooldown()
    {
        isOnCooldown = false;
        cooldownTimer = EntityConfig.CardCooldown;
        SetSelectable(true);
    }

    public void SetSelectable(bool canSelect)
    {
        isSelectable = canSelect;
        cooldownOverlay.fillAmount = canSelect ? 0f : 1f;
        cooldownOverlay.enabled = !canSelect;
    }

    bool CanSelect()
    {
        return !isOnCooldown && isSelectable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanSelect()) return;
        InputManager.Instance.OnCardSelected(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanSelect()) return;
        InputManager.Instance.OnCardSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanSelect()) return;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanSelect()) return;
        transform.localScale = Vector3.one;
    }

    public void OnDrag(PointerEventData eventData){}
    public void OnEndDrag(PointerEventData eventData){}
}