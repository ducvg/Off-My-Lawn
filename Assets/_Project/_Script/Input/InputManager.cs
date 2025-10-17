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
    private Hero previewHero;
    private Material heroMaterial;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    public void OnBeginDragCard(Card selected)
    {
        SelectedCard = selected;
        previewHero = Instantiate(SelectedCard.HeroConfig.Prefab, transform);

        heroMaterial = previewHero.GraphicController.GetHeroMaterial();
        previewMaterial.SetTexture(MainTex, heroMaterial.GetTexture(MainTex));
        previewHero.GraphicController.ChangeMaterial(previewMaterial);
    }

    public void OnDragCard()
    {
        selectedCell = null;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit groundHit, 1000f, groundLayer);
        if (Physics.Raycast(ray, out RaycastHit cellHit, 1000f, cellLayer))
        {
            selectedCell = GameGrid.Instance.GetCellAtPosition(cellHit.transform.position);
            if (selectedCell.CanPlaceHero())
                previewHero.transform.position = new Vector3(cellHit.transform.position.x, groundHit.point.y, cellHit.transform.position.z);
            else
                previewHero.transform.position = groundHit.point;
                
            return;
        }

        previewHero.transform.position = groundHit.point;
    }

    public void OnEndDragCard()
    {
        if (selectedCell != null && selectedCell.CanPlaceHero())
        {
            previewHero.GraphicController.ChangeMaterial(heroMaterial);
            selectedCell.PlaceHero(previewHero);
        }
        else
        {
            Destroy(previewHero.gameObject);
        }
        
        previewHero = null;
        SelectedCard = null;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EditorApplication.isPaused = true;
        }
    }

}
