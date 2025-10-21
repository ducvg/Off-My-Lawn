using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Data Object/Entity/Monster Config")]
public class MonsterConfigSO : EntityConfigSO<Monster>
{    
    [field: SerializeField] public int MoveSpeed { get; private set; }

    [field: Header("Spawn Data")]
    [field: SerializeField] public int SpawnCost { get; private set; }

}
