using UnityEditor;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private new Camera camera;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask cellLayer;

    public Card SelectedCard { get; private set; }
    private GameCell selectedCell = null;
    private Entity previewEntity;
    private Material heroMaterial;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

#region Card Drag & Drop
    public void OnBeginDragCard(Card selected)
    {
        SelectedCard = selected;
        previewEntity = Instantiate(SelectedCard.Config.Prefab, transform);

        heroMaterial = previewEntity.GraphicController.GetHeroMaterial();
        previewMaterial.SetTexture(MainTex, heroMaterial.GetTexture(MainTex));
        previewEntity.GraphicController.ChangeMaterialAll(previewMaterial);
    }

    public void OnDragCard()
    {
        selectedCell = null;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, groundLayer);
        if (Physics.Raycast(ray, out RaycastHit cellHit, 1000f, cellLayer))
        {
            selectedCell = GameGrid.Instance.GetCellAtPosition(cellHit.transform.position);
            if (selectedCell.CanPlace())
                previewEntity.transform.position = new Vector3(cellHit.transform.position.x, groundHit.point.y, cellHit.transform.position.z);
            else
                previewEntity.transform.position = groundHit.point;
                
            return;
        }

        previewEntity.transform.position = groundHit.point;
    }

    public void OnEndDragCard()
    {
        if (selectedCell != null && selectedCell.CanPlace())
        {
            previewEntity.GraphicController.ChangeMaterialAll(heroMaterial);
            selectedCell.Place(previewEntity);

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



#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EditorApplication.isPaused = true;
        }
    }
#endif

}
