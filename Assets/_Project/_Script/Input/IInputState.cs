public interface IInputState
{
    void OnEnter(InputManager inputManager);
    void OnUpdate(InputManager inputManager);
    void OnExit(InputManager inputManager);
}
