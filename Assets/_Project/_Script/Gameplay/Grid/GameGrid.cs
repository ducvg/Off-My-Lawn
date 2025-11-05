using UnityEngine;

public class GameGrid : Singleton<GameGrid>
{
    [SerializeField] private Vector2Int gridSize = new(11, 6);
    [SerializeField] private Grid gridMap;
    [SerializeField] private GameCell[] cellObjects;

    public GameCell[,] Grid { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Grid = new GameCell[gridSize.x, gridSize.y];
        for (int i = 0; i < cellObjects.Length; i++)
        {
            Vector3Int pos = gridMap.WorldToCell(cellObjects[i].transform.position);
            Grid[pos.x, pos.y] = cellObjects[i];
        }
        cellObjects = null;
    }

    public GameCell GetCellAtPosition(Vector3 position)
    {
        Vector3Int pos = gridMap.WorldToCell(position);
        if (pos.x < 0 || pos.x > Grid.GetLength(0) + 1 ||
            pos.y < 0 || pos.y > Grid.GetLength(1) + 1)
            return null;
        return Grid[pos.x, pos.y];
    }

    public Vector3 GetCellCenterPosition(GameCell cell)
    {
        Vector3Int pos = gridMap.WorldToCell(cell.transform.position);
        return gridMap.GetCellCenterWorld(pos);
    }

    public int GetRandomRowIndex() =>Random.Range(0, Grid.GetLength(1));
    public int GetRandomColumnIndex() =>Random.Range(0, Grid.GetLength(0));
}
