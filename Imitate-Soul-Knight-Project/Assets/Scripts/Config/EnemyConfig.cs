using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : IConfig {
    public List<EnemyConfigData> enemyList = new List<EnemyConfigData> ();

    private Dictionary<int, EnemyConfigData> enemyConfigDic = new Dictionary<int, EnemyConfigData> ();

    public EnemyConfigData getEnemyConfigById (int id) {
        return this.enemyConfigDic[id];
    }

    public void convertData () {
        foreach (EnemyConfigData enemyConfigData in enemyList) {
            this.enemyConfigDic.Add (enemyConfigData.id, enemyConfigData);
        }
    }
}