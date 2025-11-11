using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroRemover : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public Image Icon { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        InputManager.Instance.OnHeroRemoverSelected(this);
    }
}
