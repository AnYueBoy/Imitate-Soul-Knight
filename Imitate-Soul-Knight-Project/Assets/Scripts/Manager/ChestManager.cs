/*
 * @Author: l hy 
 * @Date: 2021-11-19 17:57:35 
 * @Description: 宝箱管理
 */
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class ChestManager {
    private HashSet<BaseChest> chestSet = new HashSet<BaseChest> ();

    public void localUpdate (float dt) {
        foreach (BaseChest chest in chestSet) {
            chest.localUpdate (dt);
        }
    }

    public void spawnChest (Vector3 position,string chestUrl) {
        GameObject chestPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (chestUrl);
        GameObject chestNode = ObjectPool.instance.requestInstance (chestPrefab);

        chestNode.transform.position = position;
        chestNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        BaseChest chest = chestNode.GetComponent<BaseChest> ();
        this.chestSet.Add (chest);
    }
}