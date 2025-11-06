
using System.Runtime.CompilerServices;
using UnityEngine;

public struct WalkState : IState
{
    float randSpeed; //desync movement
    float newBaseSpeed;

    public void OnEnter(Entity entity)
    {
        randSpeed = Random.Range(-0.05f, 0.05f);
        newBaseSpeed = entity.StatModifier.GetFinalMoveSpeed() + randSpeed;

        entity.GraphicController.Animator.SetFloat(Animation.MoveSpeedHash, newBaseSpeed);
        entity.GraphicController.PlayAnimation(Animation.MoveHash, 0.1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        float curveFactor = entity.Config.SpeedCurve.Evaluate(Time.time * newBaseSpeed);
        float finalSpeed = newBaseSpeed * curveFactor;
        entity.transform.Translate(Vector3.forward * finalSpeed * Time.deltaTime);

        if (entity.EquipmentController.Weapon.HasTarget())
        {
            entity.ChangeState(new AttackState());
        } 
    }

    public void OnExit(Entity entity)
    {
    }
}