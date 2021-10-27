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

    [SerializeField]
    protected SpriteRenderer shotGunEffect;

    protected float launchInterval = 0;

    protected float launchTimer = 0;

    protected WeaponConfigData weaponConfigData;

    protected string weaponTag;

    protected float shotGunTimer = 0;

    public virtual void init (string tag) {
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);
        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;
        this.weaponTag = tag;
        this.shotGunEffect.gameObject.SetActive (false);
    }

    public virtual void localUpdate (float dt) {
        this.launchTimer += dt;

        this.refreshShotGun (dt);
    }

    protected void refreshShotGun (float dt) {
        if (!this.shotGunEffect.gameObject.activeSelf) {
            return;
        }

        this.shotGunTimer += dt;
        this.shotGunEffect.color = new Color (1, 1, 1, (1 / this.launchInterval / 2) * this.shotGunTimer);
        if (this.shotGunTimer > this.launchInterval / 3) {
            this.shotGunEffect.gameObject.SetActive (false);
            this.shotGunTimer = 0;
        }
    }

    public virtual void launchBullet (float bulletDir) {
        if (this.launchTimer < this.launchInterval) {
            return;
        }

        this.launchTimer = 0;
        ModuleManager.instance.bulletManager.spawnBullet (launchTrans, bulletDir, this.weaponTag, this.weaponConfigData);

        this.shotGunEffect.gameObject.SetActive (true);
        this.shotGunEffect.color = new Color (1, 1, 1, 1);
    }
}