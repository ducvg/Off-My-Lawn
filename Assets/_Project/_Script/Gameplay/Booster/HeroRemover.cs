using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroRemover : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Texture2D removerCursorTexture;

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isActive = IsRemoving();
        if (!isActive)
        {
            InputManager.Instance.ChangeInputState(new RemoveHeroInputState()
            {
                Icon = icon,
                CursorTexture = removerCursorTexture
            });
        }
        else
        {
            InputManager.Instance.ChangeInputState(new EmptyInputState());
        }
    }

    bool IsRemoving()
    {
        return InputManager.Instance.CurrentInputState is RemoveHeroInputState;
    }
}
