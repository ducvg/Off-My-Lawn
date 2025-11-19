using System.Runtime.CompilerServices;
using UnityEngine;

public class GenericCurvedProjectile : Projectile
{
    [SerializeField] private Transform GraphicTransform;
    float traveledDistance;
    float targetDistance;
    Vector3 initialPosition;
    float relativeDistance;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        initialPosition = transform.position;
        float realAttackRange = Mathf.Min(weapon.Config.AttackRange, GameConstant.GRID_BOUND_X_MAX);
        
        bool isHit = Physics.Raycast(initialPosition, transform.forward, out RaycastHit hitInfo, realAttackRange, weapon.TargetLayerMask);
        if (isHit) targetDistance = hitInfo.distance;
        else targetDistance = realAttackRange;
        
        relativeDistance = targetDistance / GameConstant.GRID_BOUND_X_MAX;
        traveledDistance = 0f;
    }

    public override void OnMove()
    {
        if(traveledDistance > targetDistance)
        {
            ProjectileManager.Instance.ToDespawn(this);
            return;
        }
        base.OnMove();
        traveledDistance += Config.Speed * Time.deltaTime;
        float height = initialPosition.y + Config.HeightCurve.Evaluate(traveledDistance / targetDistance) * relativeDistance;

        Vector3 flat = initialPosition + transform.forward * traveledDistance;
        transform.position = new Vector3(flat.x, height, flat.z);

        RotateGraphicDirection();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RotateGraphicDirection()
    {
        Vector3 dir = LastPosition - transform.position;
        GraphicTransform.rotation = Quaternion.LookRotation(dir);
    }
}