using UnityEditor;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Shader previewShader;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private FloatValueSO crystalValue;
    public Card SelectedCard { get; private set; }
    private Shader litURP;
    private GameCell selectedCell;
    private Entity previewEntity;
    private new Camera camera;

    void Start()
    {
        litURP = Shader.Find("Universal Render Pipeline/Lit");
        camera = Camera.main;
        Input.multiTouchEnabled = false;
    }

    #region Card Drag & Drop
    public void OnBeginDragCard(Card selected)
    {
        if (crystalValue.Value < selected.EntityConfig.CrystalCost)
        {
            UIManager.Instance.GetCanvas<GameplayCanvas>().WarnInsufficientCrystal();
            return;
        }
        SelectedCard = selected;
        previewEntity = Instantiate(SelectedCard.EntityConfig.Prefab);
        previewEntity.GraphicController.SetShaderAll(previewShader);
    }

    public void OnDragCard()
    {
        if(!SelectedCard) return;
        selectedCell = null;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, groundLayer);
        if (Physics.Raycast(ray, out RaycastHit cellHit, 1000f, cellLayer))
        {
            Transform cellTf = cellHit.transform;
            selectedCell = GameGrid.Instance.GetCellAtPosition(cellTf.position);
            if (selectedCell.CanPlace())
                previewEntity.transform.position = new Vector3(cellTf.position.x, groundHit.point.y, cellTf.position.z);
            else
                previewEntity.transform.position = groundHit.point;
                
            return;
        }

        previewEntity.transform.position = groundHit.point;
    }

    public void OnEndDragCard()
    {
        if (!SelectedCard) return;
        if (selectedCell != null && selectedCell.CanPlace())
        {
            previewEntity.Init(SelectedCard.EntityConfig);
            previewEntity.GraphicController.SetShaderAll(litURP);
            // previewEntity.GraphicController.ChangeMaterialAll(heroMaterial);
            selectedCell.Place(previewEntity);
            crystalValue.Value -= SelectedCard.EntityConfig.CrystalCost;
            selectedCell = null;
        }
        else
        {
            Destroy(previewEntity.gameObject);
        }

        previewEntity = null;
        SelectedCard = null;
    }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EditorApplication.isPaused = true;
        }

        // if(Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit cellHit, 100f, cellLayer))
        //     {
        //         selectedCell = GameGrid.Instance.GetCellAtPosition(cellHit.transform.position);
        //         if(selectedCell) Debug.Log($"Clicked Cell", selectedCell);
        //     }
        // }
    }
}
