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

    protected WeaponConfigData weaponConfigData;

    protected string weaponTag;

    #region  枪口火焰
    protected float shotGunEffectInterval;

    protected float shotGunEffectSpeed;
    protected float shotGunTimer = 0;

    #endregion

    #region  发射
    protected float launchInterval = 0;

    protected float launchTimer = 0;

    #endregion

    #region  后坐力
    protected readonly float recoilForeceDis = 0.012f;
    protected float recoilForceTimer = 0;
    protected float recoilForceSpeed;
    protected float recoilForceInterval;
    protected bool isRecoilForce = false;
    #endregion

    public virtual void init (string tag) {
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);
        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;
        this.weaponTag = tag;
        this.shotGunEffect.gameObject.SetActive (false);

        this.shotGunEffectInterval = this.launchInterval / 2;
        this.shotGunEffectSpeed = 1 / shotGunEffectInterval;

        this.recoilForceInterval = (this.launchInterval / 2) / 2;
        this.recoilForceSpeed = this.recoilForeceDis / this.recoilForceInterval;
    }

    public virtual void localUpdate (float dt) {
        this.launchTimer += dt;

        this.refreshShotGun (dt);

        this.recoilForce(dt);
    }

    protected void refreshShotGun (float dt) {
        if (!this.shotGunEffect.gameObject.activeSelf) {
            return;
        }

        this.shotGunTimer += dt;
        this.shotGunEffect.color = new Color (1, 1, 1, this.shotGunEffectSpeed * this.shotGunTimer);
        if (this.shotGunTimer > this.shotGunEffectInterval) {
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

        this.isRecoilForce = true;
    }

    protected virtual void recoilForce (float dt) {
        if (!this.isRecoilForce) {
            return;
        }
        this.recoilForceTimer += dt;
        if (this.recoilForceTimer > this.recoilForceInterval) {
            this.recoilForceTimer = 0;
            if (this.recoilForceSpeed < 0) {
                this.transform.localPosition = Vector3.zero;
                this.isRecoilForce = false;
            }
            this.recoilForceSpeed = -this.recoilForceSpeed;
        }
        float nodeX = this.transform.localPosition.x;
        this.transform.localPosition = new Vector3 (nodeX - this.recoilForceTimer * this.recoilForceSpeed, 0, 0);

    }
}