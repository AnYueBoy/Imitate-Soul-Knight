/*
 * @Author: l hy 
 * @Date: 2021-10-27 18:09:20 
 * @Description: 敌人管理
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UFramework.GameCommon;
using UnityEngine;

public class EnemyManager : IEnemyManager {

    private HashSet<BaseEnemy> enemySet = new HashSet<BaseEnemy> ();

    public void LocalUpdate (float deltaTime) {
        foreach (BaseEnemy enemy in enemySet) {
            enemy.localUpdate (deltaTime);
        }
    }

    public BaseEnemy SpawnEnemyById (int enemyId, Vector3 pos, Func<bool> callback = null) {
        EnemyConfigData enemyConfigData = App.Make<IConfigManager> ().EnemyConfig.getEnemyConfigById (enemyId);
        string enemyUrl = enemyConfigData.url;
        GameObject enemyPrefab = App.Make<IAssetsManager> ().GetAssetByUrlSync<GameObject> (enemyUrl);
        GameObject enemyNode = App.Make<IObjectPool> ().RequestInstance (enemyPrefab);
        enemyNode.transform.SetParent (App.Make<ITransfromManager> ().GoTransfrom);
        enemyNode.transform.position = pos;

        BaseEnemy enemy = enemyNode.GetComponent<BaseEnemy> ();
        enemy.init (enemyConfigData, callback);
        enemySet.Add (enemy);
        return enemy;
    }
}