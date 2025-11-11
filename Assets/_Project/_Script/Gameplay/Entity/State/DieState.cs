
using System.Runtime.CompilerServices;
using PrimeTween;

public struct DieState : IState
{
    Tween dieTween;

    public void OnEnter(Entity entity)
    {
        entity.SetColliderActive(false);
        entity.GraphicController.PlayAnimation(Animation.DieHash, 0.3f);
        
        dieTween = Tween.PositionY(entity.transform, endValue: -1f, duration: 2f, startDelay: GameConstant.OBJECT_DESPAWN_TIME)
            .OnComplete(entity, target => target.Despawn());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {}

    public void OnExit(Entity entity)
    {
        dieTween.Complete();
    }
}