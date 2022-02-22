using System;
using UnityEngine;
public interface IEnemyManager {
    void LocalUpdate (float deltaTime);

    BaseEnemy SpawnEnemyById (int enemyId, Vector3 pos, Func<bool> callback = null);
}