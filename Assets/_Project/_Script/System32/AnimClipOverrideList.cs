using System.Collections.Generic;
using UnityEngine;
using ZLinq;

public class AnimClipOverrideList : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimClipOverrideList(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

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
