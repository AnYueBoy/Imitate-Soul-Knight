/*
 * @Author: l hy 
 * @Date: 2021-10-22 18:24:05 
 * @Description: 武器基类
 */
using UFramework;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    [SerializeField]
    private Transform launchTrans;

    [SerializeField]
    protected int id;

    protected float launchInterval = 0;

    protected float launchTimer = 0;

    protected WeaponConfigData weaponConfigData;

    public virtual void init () {
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);
        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;
    }

    public virtual void localUpdate (float dt) {
        this.launchTimer += dt;
    }

    public virtual void launchBullet (float bulletDir) {
        if (this.launchTimer < this.launchInterval) {
            return;
        }
        this.launchTimer = 0;
        ModuleManager.instance.bulletManager.spawnBullet (launchTrans, bulletDir, this.weaponConfigData);
    }
}