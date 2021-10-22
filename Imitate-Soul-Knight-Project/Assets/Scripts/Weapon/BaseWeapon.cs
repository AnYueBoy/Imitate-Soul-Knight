/*
 * @Author: l hy 
 * @Date: 2021-10-22 18:24:05 
 * @Description: 武器基类
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    [SerializeField]
    private Transform launchTrans;

    [SerializeField]
    protected string bulletUrl = "";

    protected void launchBullet () {
        GameObject bulletPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (this.bulletUrl);
        GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);
        
    }
}