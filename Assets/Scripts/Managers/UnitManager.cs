using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> Units;

    public BaseHero SelectedHero;
    public BaseEnemy SelectedEnemy;

    void Awake()
    {
        Instance = this;

        Units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
    {
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero, Vector3.zero);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 10;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);

            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
            randomSpawnTile.SetUnit(spawnedEnemy, Vector3.zero);

        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }



    public T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)Units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
    }

    public BaseHero GetSelectedHero()
    {
        return this.SelectedHero;
    }
}
