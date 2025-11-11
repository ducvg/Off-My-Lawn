using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public struct PlaceHeroInputState : IInputState
{
    public LayerMask GroundLayer, CellLayer;
    public Shader PreviewShader, DefaultShader;
    public Card SelectedCard;
    public FloatValueSO CrystalValue;
    Hero previewHero;
    GameCell selectedCell;
    Camera camera;

    public void OnEnter()
    {
        if (SelectedCard.EntityConfig.Prefab is not Hero hero)
        {
            InputManager.Instance.ChangeInputState(new EmptyInputState());
            return;
        }

        if (CrystalValue.Value < SelectedCard.EntityConfig.CrystalCost)
        {
            UIManager.Instance.GetCanvas<GameplayCanvas>().WarnInsufficientCrystal();
            InputManager.Instance.ChangeInputState(new EmptyInputState());
            return;
        }

        camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, GroundLayer);
        previewHero = Object.Instantiate(hero);
        previewHero.transform.position = groundHit.point;
        previewHero.GraphicController.SetShaderAll(PreviewShader);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0)) //click mode
        {
            InputManager.Instance.ChangeInputState(new EmptyInputState());
            return;
        }
        
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, GroundLayer);
        if (Physics.Raycast(ray, out RaycastHit cellHit, 1000f, CellLayer))
        {
            Transform cellTf = cellHit.transform;
            selectedCell = GameGrid.Instance.GetCellAtPosition(cellTf.position);
            if (selectedCell.CanPlace())
                previewHero.transform.position = new Vector3(cellTf.position.x, groundHit.point.y, cellTf.position.z);
            else
                previewHero.transform.position = groundHit.point;

            if (Input.GetMouseButtonUp(0)) //drag mode release
            {
                InputManager.Instance.ChangeInputState(new EmptyInputState());
            }
            
            return;
        }
        else
        {
            selectedCell = null;
        }
        
        previewHero.transform.position = groundHit.point;
    }

    public void OnExit()
    {
        if (selectedCell && selectedCell.CanPlace())
        {
            CrystalValue.Value -= SelectedCard.EntityConfig.CrystalCost;

            previewHero.Init(SelectedCard.EntityConfig);
            previewHero.GraphicController.SetShaderAll(DefaultShader);

            selectedCell.PlaceHero(previewHero);
            selectedCell = null;
            SelectedCard.StartCooldown();
        }
        else
        {
            if(previewHero) Object.Destroy(previewHero.gameObject);
        }

        previewHero = null;
        SelectedCard = null;
    }
}
