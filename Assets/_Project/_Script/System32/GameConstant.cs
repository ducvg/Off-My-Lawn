using UnityEngine;

public class GameConstant
{
    public const float LAWN_ELEVATION_Y = 0.1f;
    public const float GRID_BOUND_X_MAX = 12f;
    public const float GRID_BOUND_X_MIN = -4f;
    public const float GRID_BOUND_Y_MIN = 0;
    
    public const float MONSTER_SPAWN_RANGE_X = 4f;
    public const float ENTITY_DESPAWN_TIME = 3f;
    public const float OBJECT_DESPAWN_TIME = 5f;

    public static readonly int emissionId = Shader.PropertyToID("_EmissionColor");
    public static readonly int colorId = Shader.PropertyToID("_Color");

}