using UnityEngine;

public class Animation
{
    public static readonly int SpawnGroundHash = Animator.StringToHash(SPAWN_GROUND);
    public static readonly int SpawnAirHash = Animator.StringToHash(SPAWN_AIR);
    public static readonly int IdleHash = Animator.StringToHash(IDLE);
    public static readonly int MoveHash = Animator.StringToHash(MOVE);
    public static readonly int EquipHash = Animator.StringToHash(EQUIP);
    public static readonly int AttackHash = Animator.StringToHash(ATTACK);
    public static readonly int HurtHash = Animator.StringToHash(HURT);
    public static readonly int DieHash = Animator.StringToHash(DIE);

    public const string SPAWN_GROUND = "Spawn_Ground";
    public const string SPAWN_AIR = "Spawn_Air";
    public const string IDLE = "Idle";
    public const string MOVE = "Move";
    public const string EQUIP = "Equip";
    public const string ATTACK = "Attack";
    public const string HURT = "Hurt";
    public const string DIE = "Die";

    public static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    public static readonly int AttackSpeedHash = Animator.StringToHash("AttackSpeed");
}
