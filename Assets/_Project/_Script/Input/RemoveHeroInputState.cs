using UnityEngine;
using UnityEngine.UI;

public struct RemoveHeroInputState : IInputState
{
    public LayerMask CellLayer;
    public Texture2D CursorTexture;
    public Image Icon;

    public void OnEnter()
    {
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
        Icon.enabled = false;
    }
    
    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit cellHit, 100f, CellLayer)) return;

            GameCell selectedCell = GameGrid.Instance.GetCellAtPosition(cellHit.transform.position);
            if (selectedCell && selectedCell.Hero)
            {
                selectedCell.Hero.Despawn();
            }
            InputManager.Instance.ChangeInputState(new EmptyInputState());
        }
    }

    public void OnExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Icon.enabled = true;
    }
}