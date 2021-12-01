/*
 * @Author: l hy 
 * @Date: 2021-10-22 18:24:05 
 * @Description: 武器基类
 */
using DG.Tweening;
using UFramework;
using UFramework.FrameUtil;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    [SerializeField]
    private Transform launchTrans;

    [SerializeField]
    protected ItemIdEnum id;

    [SerializeField]
    protected SpriteRenderer shotGunEffect;

    protected WeaponConfigData weaponConfigData;

    protected string weaponTag;

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

    public virtual void init (string bulletTag) {
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);
        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;
        this.weaponTag = bulletTag;

        this.shotGunEffect.color = new Color (1, 1, 1, 0);

        this.recoilForceInterval = (this.launchInterval / 2) / 2;
        this.recoilForceSpeed = this.recoilForeceDis / this.recoilForceInterval;
    }

    public virtual void localUpdate (float dt) {
        this.launchTimer += dt;
        this.recoilForce (dt);
    }

    private readonly float shotGunTime = 0.2f;

    protected void spawnShotGunFire () {
        this.shotGunEffect.color = new Color (1, 1, 1, 0);
        this.shotGunEffect
            .DOColor (new Color (1, 1, 1, 1), shotGunTime / 2)
            .OnComplete (() => {
                this.shotGunEffect.color = new Color (1, 1, 1, 0);
            });

    }

    public virtual void launchBullet (float bulletDir) {
        if (this.launchTimer < this.launchInterval) {
            return;
        }

        this.spawnShotGunFire ();

        float weaponConsumeValue = this.weaponConfigData.mpConsume;
        float curMp = ModuleManager.instance.playerManager.getCurMp ();
        if (curMp < weaponConsumeValue) {
            return;
        }
        ModuleManager.instance.playerManager.consumeMp (weaponConsumeValue);

        this.launchTimer = 0;
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        float bulletSpeed = this.weaponConfigData.bulletSpeed;
        if (moveDir != Vector2.zero) {
            bulletSpeed += ConstValue.moveSpeed;
        }

        // 产生子弹偏移
        float randomValue = CommonUtil.getRandomValue (-1, 1);
        Vector3 endEulerAngles = new Vector3 (launchTrans.eulerAngles.x, launchTrans.eulerAngles.y, launchTrans.eulerAngles.z + randomValue * this.weaponConfigData.bulletOffset);

        ModuleManager.instance.bulletManager.spawnBullet (
            launchTrans.position,
            endEulerAngles,
            bulletDir,
            this.weaponTag,
            this.weaponConfigData.bulletUrl,
            bulletSpeed,
            this.weaponConfigData.damage);

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

    public float getWeaponMpConsume () {
        return this.weaponConfigData.mpConsume;
    }
}