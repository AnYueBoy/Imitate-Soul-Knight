using System;
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

    public BaseEnemy spawnEnemyById (int enemyId, Vector3 pos, Func<bool> callback = null) {
        EnemyConfigData enemyConfigData = ModuleManager.instance.configManager.enemyConfig.getEnemyConfigById (enemyId);
        string enemyUrl = enemyConfigData.url;
        GameObject enemyPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (enemyUrl);
        GameObject enemyNode = ObjectPool.instance.requestInstance (enemyPrefab);
        enemyNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
        enemyNode.transform.position = pos;

        BaseEnemy enemy = enemyNode.GetComponent<BaseEnemy> ();
        enemy.init (enemyConfigData, callback);
        enemySet.Add (enemy);
        return enemy;
    }

    public BaseEnemy getClosetEnemy () {
        BaseEnemy closestEnemy = null;
        Transform playerTrans = ModuleManager.instance.playerManager.getPlayerTrans ();
        float curClosetDis = ConstValue.playerAttackDis;
        foreach (BaseEnemy enemy in enemySet) {
            if (enemy == null) {
                continue;
            }

            if (!enemy.transform.gameObject.activeSelf) {
                continue;
            }

            float distance = (enemy.transform.position - playerTrans.position).magnitude;
            if (distance > ConstValue.playerAttackDis) {
                continue;
            }

            if (distance <= curClosetDis) {
                curClosetDis = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}