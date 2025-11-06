using UnityEngine;
using UnityEngine.UI;

public struct RemoveHeroInputState : IInputState
{
    public Texture2D CursorTexture;
    public Image Icon;

    public void OnEnter(InputManager inputManager)
    {
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
        Icon.enabled = false;
    }
    
    public void OnUpdate(InputManager inputManager)
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (!Physics.Raycast(ray, out RaycastHit cellHit, 100f, InputManager.Instance.CellLayer)) return;
        // cellHit.Hero.GraphicController.ChangeShaderAll("/Hightlight"));
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit cellHit, 100f, InputManager.Instance.CellLayer)) return;

            GameCell selectedCell = GameGrid.Instance.GetCellAtPosition(cellHit.transform.position);
            if (selectedCell && selectedCell.Hero)
            {
                selectedCell.Hero.Despawn();
            }
            inputManager.ChangeInputState(new EmptyInputState());
        }
    }

    public void OnExit(InputManager inputManager)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Icon.enabled = true;
    }
}