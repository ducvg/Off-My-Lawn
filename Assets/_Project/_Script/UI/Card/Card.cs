using Cysharp.Text;
using PrimeTween;
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
    public EntityConfigSO EntityConfig { get; private set; }
    float cooldownTimer;
    bool isOnCooldown;

    public void Init(EntityConfigSO config)
    {
        EntityConfig = config;
        EndCooldown();

        icon.sprite = config.Icon;
        costText.SetTextFormat("{0}", config.CrystalCost);
        background.color = config.CardColor;
    }

    void Update()
    {
        CooldownUpdate();
    }

    public void CooldownUpdate()
    {
        if (!isOnCooldown) return;

        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= EntityConfig.CardCooldown)
        {
            EndCooldown();
            return;
        }
        cooldownOverlay.fillAmount = 1f - (cooldownTimer / EntityConfig.CardCooldown);
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = 0f;
        cooldownOverlay.enabled = true;
        cooldownOverlay.fillAmount = 1f;
    }
    
    void EndCooldown()
    {
        isOnCooldown = false;
        cooldownTimer = EntityConfig.CardCooldown;
        cooldownOverlay.enabled = false;
        cooldownOverlay.fillAmount = 0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOnCooldown) return;
        InputManager.Instance.OnCardSelected(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOnCooldown) return;
        InputManager.Instance.OnCardSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOnCooldown) return;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOnCooldown) return;
        transform.localScale = Vector3.one;
    }

    public void OnEndDrag(PointerEventData eventData) //required by beginHandler
    {
    }

    public void OnDrag(PointerEventData eventData) //required by beginHandler
    {
    }
}