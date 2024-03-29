using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int Width, Height;

    [SerializeField] private Tile GrassTile, BlockedTile;

    private Dictionary<Vector2, Tile> Tiles;

    private void Awake()
    {
        Instance = this;
    }
    public void GenerateGrid()
    {
        Tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                var randomTile = Random.Range(0, 10) >= 8 ? BlockedTile : GrassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x}{y}";

                spawnedTile.Init(x, y);

                Tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        return Tiles.Where(t => t.Key.x < Width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return Tiles.Where(t => t.Key.x > Width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (Tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
