using System.Runtime.CompilerServices;

public struct StunState : IState
{
    public void OnEnter(Entity entity)
    {
        entity.GraphicController.Animator.speed = 0f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
    }

    public void OnExit(Entity entity)
    {
        entity.GraphicController.Animator.speed = 1f;
    }
}
