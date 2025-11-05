
using System.Runtime.CompilerServices;
using UnityEngine;

public struct WalkState : IState
{
    float randSpeed;

    public void OnEnter(Entity entity)
    {
        randSpeed = Random.Range(-0.05f, 0.05f);
        entity.GraphicController.Animator.SetFloat(Animation.MoveSpeedHash, entity.StatBonus.GetFinalMoveSpeed() + randSpeed);
        entity.GraphicController.PlayAnimation(Animation.MoveHash, 0.1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        float baseSpeed = entity.StatBonus.GetFinalMoveSpeed() + randSpeed;
        float curveFactor = entity.Config.SpeedCurve.Evaluate(Time.time * baseSpeed);
        float finalSpeed = baseSpeed * curveFactor;
        entity.transform.Translate(Vector3.forward * finalSpeed * Time.deltaTime);

        if (entity.EquipmentController.Weapon.HasTargetInRange())
        {
            entity.ChangeState(new AttackState());
        } 
    }

    public void OnExit(Entity entity)
    {
    }
}