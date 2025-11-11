using System.Runtime.CompilerServices;

public struct AttackState : IState
{

    public void OnEnter(Entity entity)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        var weapon = entity.EquipmentController.Weapon;

        if (!weapon.IsAttackFinished()) return;
        if (!weapon.HasTarget())
        {
            entity.ChangeState(new IdleState());
            return;
        }
        if (weapon.IsOnCooldown()) return;
        
        entity.GraphicController.PlayAnimation(Animation.AttackHash, 0.1f);
        weapon.Attack(entity);
    }

    public void OnExit(Entity entity)
    {
        var weapon = entity.EquipmentController.Weapon;

        weapon.CancelAttack();
    }
}

