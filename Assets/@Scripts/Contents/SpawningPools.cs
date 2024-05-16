using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPools : MonoBehaviour
{
    public int MaxMonsterCount = 1000;
    Coroutine _coUpdateSpawningPool;
    GameManager _game;

    public void StartSpawn()
    {
        _game = Managers.Game;
        if (_coUpdateSpawningPool != null)
        {
            _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
        }
    }

    IEnumerator CoUpdateSpawningPool()
    {
        // while (true)
        // {
        //     if (_game.CurrentWaveData.MonsterId.Count == 1)
        //     {
        //         for (int i = 0; i < _game.CurrentWaveData.OnceSpawnCount; i++)
        //         {
        //             Vector2 spawnPos = Util.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
        //             Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[0]);
        //         }
        //         yield return new WaitForSeconds(_game.CurrentWaveData.SpawnInterval);
        //     }
        //     else
        //     {
        //         for (int i = 0; i < _game.CurrentWaveData.OnceSpawnCount; i++)
        //         {
        //             Vector2 spawnPos = Util.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);

        //             if (Random.value <= Managers.Game.CurrentWaveData.FirstMonsterSpawnRate) // 90%의 확률로 첫번째 MonsterId 사용
        //             {
        //                 Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[0]);
        //             }
        //             else // 10%의 확률로 다른 MonsterId 사용
        //             {
        //                 int randomIndex = Random.Range(1, _game.CurrentWaveData.MonsterId.Count);
        //                 Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[randomIndex]);
        //             }
        //         }
        //         yield return new WaitForSeconds(_game.CurrentWaveData.SpawnInterval);
        //     }
        // }
        yield return null;
    }
}
