using UnityEditor;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Shader previewShader;
    [SerializeField] private FloatValueSO crystalValue;
    [SerializeField] private Texture2D removerCursorTexture;
    private Shader litURP;
    private IInputState currentInputState;

    void Start()
    {
        litURP = Shader.Find("Universal Render Pipeline/Lit");
        Input.multiTouchEnabled = false;
        currentInputState = new EmptyInputState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        currentInputState.OnUpdate();
    }

    public void ChangeInputState(IInputState newState)
    {
        if (currentInputState != null)
        {
            currentInputState.OnExit();
        }
        currentInputState = newState;
        if (currentInputState != null)
        {
            currentInputState.OnEnter();
        }
    }

    public void OnCardSelected(Card selected)
    {
        if (currentInputState is PlaceHeroInputState) return;
        
        ChangeInputState(new PlaceHeroInputState
        {
            GroundLayer = groundLayer,
            CellLayer = cellLayer,
            SelectedCard = selected,
            PreviewShader = previewShader,
            DefaultShader = litURP,
            CrystalValue = crystalValue
        });
    }
    
    public void OnHeroRemoverSelected(HeroRemover remover)
    {
        bool isActive = currentInputState is RemoveHeroInputState;
        if (!isActive)
        {
            ChangeInputState(new RemoveHeroInputState()
            {
                CellLayer = cellLayer,
                Icon = remover.Icon,
                CursorTexture = removerCursorTexture
            });
        }
        else
        {
            ChangeInputState(new EmptyInputState());
        }
    }
}