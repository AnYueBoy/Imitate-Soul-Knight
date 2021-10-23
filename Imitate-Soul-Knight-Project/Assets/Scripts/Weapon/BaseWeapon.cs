/*
 * @Author: l hy 
 * @Date: 2021-10-22 18:24:05 
 * @Description: 武器基类
 */
using System.Collections;
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    [SerializeField]
    private Transform launchTrans;

    [SerializeField]
    protected string bulletUrl = "";

    private readonly float bulletSpeed = 3f;

    private readonly float launchInterval = 0.5f;

    private float launchTimer = 0.5f;

    public void localUpdate (float dt) {
        this.launchTimer += dt;
    }

    public void launchBullet (Vector3 bulletDir) {
        if (this.launchTimer < this.launchInterval) {
            return;
        }
        this.launchTimer = 0;
        ModuleManager.instance.bulletManager.spawnBullet (this.bulletUrl, launchTrans, bulletSpeed, bulletDir);
    }
}