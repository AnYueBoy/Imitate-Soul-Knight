/*
 * @Author: l hy 
 * @Date: 2021-10-27 18:09:20 
 * @Description: 敌人管理
 */

using System.Collections;
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class EnemyManager {

    private HashSet<BaseEnemy> enemySet = new HashSet<BaseEnemy> ();

    public void localUpdate (float dt) {
        foreach (BaseEnemy enemy in enemySet) {
            enemy.localUpdate (dt);
        }
    }

    public BaseEnemy spawnEnemyById (int enemyId, Vector3 pos) {
        EnemyConfigData enemyConfigData = ModuleManager.instance.configManager.enemyConfig.getEnemyConfigById (enemyId);
        string enemyUrl = enemyConfigData.url;
        GameObject enemyPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (enemyUrl);
        GameObject enemyNode = ObjectPool.instance.requestInstance (enemyPrefab);
        enemyNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
        enemyNode.transform.position = pos;

        BaseEnemy enemy = enemyNode.GetComponent<BaseEnemy> ();
        enemySet.Add (enemy);
        return enemy;
    }
}