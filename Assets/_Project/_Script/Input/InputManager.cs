using UnityEditor;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [field: SerializeField] public LayerMask CellLayer { get; private set; }
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Shader previewShader;
    [SerializeField] private FloatValueSO crystalValue;
    public Card SelectedCard { get; private set; }
    private Shader litURP;
    private GameCell selectedCell;
    private Hero previewHero;
    private new Camera camera;
    public IInputState CurrentInputState { get; private set; } = new EmptyInputState();

    void Start()
    {
        litURP = Shader.Find("Universal Render Pipeline/Lit");
        camera = Camera.main;
        Input.multiTouchEnabled = false;
    }

    #region Card Drag & Drop
    public void OnBeginDragCard(Card selected)
    {
        if (selected.EntityConfig.Prefab is not Hero hero) return;
        if (crystalValue.Value < selected.EntityConfig.CrystalCost)
        {
            UIManager.Instance.GetCanvas<GameplayCanvas>().WarnInsufficientCrystal();
            return;
        }
        SelectedCard = selected;
        previewHero = Instantiate(hero);
        previewHero.GraphicController.SetShaderAll(previewShader);
    }

    public void OnDragCard()
    {
        if (!SelectedCard) return;
        selectedCell = null;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, groundLayer);
        if (Physics.Raycast(ray, out RaycastHit cellHit, 1000f, CellLayer))
        {
            Transform cellTf = cellHit.transform;
            selectedCell = GameGrid.Instance.GetCellAtPosition(cellTf.position);
            if (selectedCell.CanPlace())
                previewHero.transform.position = new Vector3(cellTf.position.x, groundHit.point.y, cellTf.position.z);
            else
                previewHero.transform.position = groundHit.point;

            return;
        }

        previewHero.transform.position = groundHit.point;
    }

    public void OnEndDragCard()
    {
        if (!SelectedCard) return;
        if (selectedCell != null && selectedCell.CanPlace())
        {
            crystalValue.Value -= SelectedCard.EntityConfig.CrystalCost;

            previewHero.Init(SelectedCard.EntityConfig);
            previewHero.GraphicController.SetShaderAll(litURP);

            selectedCell.PlaceHero(previewHero);
            selectedCell = null;
            SelectedCard.StartCooldown();
        }
        else
        {
            Destroy(previewHero.gameObject);
        }

        previewHero = null;
        SelectedCard = null;
    }
    #endregion

    [SerializeField] private Texture2D removerCursorTexture;
    #region Hero Remover Drag & Drop


    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EditorApplication.isPaused = true;
        }
        CurrentInputState.OnUpdate(this);
    }
    
    public void ChangeInputState(IInputState newState)
    {
        if (CurrentInputState != null)
        {
            CurrentInputState.OnExit(this);
        }
        CurrentInputState = newState;
        if (CurrentInputState != null)
        {
            CurrentInputState.OnEnter(this);
        }
    }
}