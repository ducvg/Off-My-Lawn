using System;
using UnityEngine;

[Serializable]
public class SummonAttack : IAttackEffect
{
    [SerializeField] private EntityConfigSO summonEntityConfig;

    public void Apply(Entity holder)
    {
        var grid = GameGrid.Instance;
        var cellSize = grid.GetGridCellSize();
        var holderTransform = holder.transform;

        Span<Vector3> offsets = stackalloc Vector3[4];
        offsets[0] = Vector3.forward * cellSize.z; //up
        offsets[1] = Vector3.back * cellSize.z; //down
        offsets[2] = Vector3.left * cellSize.x; //left
        offsets[3] = Vector3.right * cellSize.x; //right

        var holderRowIndex = grid.GetRowIndexOf(holderTransform.position);
        if (holderRowIndex <= 0)
        {
            offsets[1] = Vector3.zero;
        }
        if (holderRowIndex >= grid.Grid.GetLength(1) - 1)
        {
            offsets[0] = Vector3.zero;
        }
        
        for (int i = 0; i < 4; i++)
        {
            var spawnPos = holderTransform.position + offsets[i];
            var entity = EntityFactory.Instance.Spawn(summonEntityConfig.Id, spawnPos);
            entity.transform.forward = holderTransform.forward;
            entity.ChangeState(new GroundRiseState());
        }
    }
}