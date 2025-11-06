public interface IState
{
    void OnEnter(Entity entity);
    void OnUpdate(Entity entity);
    void OnExit(Entity entity);
}