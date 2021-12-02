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
    protected float recoilForceDis;

    /// <summary>
    ///后坐力作用时间 
    /// </summary>
    protected readonly float recoilForceInterval = 0.3f;
    #endregion

    public virtual void init (int id) {
        this.id = (ItemIdEnum) id;
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);
        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;

        this.shotGunEffect.color = new Color (1, 1, 1, 0);

        this.recoilForceDis = this.weaponConfigData.recoilForceDis;
    }

    public virtual void equipment (string bulletTag) {
        this.weaponTag = bulletTag;
    }

    public virtual void localUpdate (float dt) {
        this.launchTimer += dt;
    }

    private readonly float shotGunInterval = 0.2f;

    protected virtual void spawnShotGunFire () {
        this.shotGunEffect.color = new Color (1, 1, 1, 0);
        this.shotGunEffect
            .DOColor (new Color (1, 1, 1, 1), shotGunInterval / 2)
            .OnComplete (() => {
                this.shotGunEffect.color = new Color (1, 1, 1, 0);
            });

    }

    protected virtual void spawnRecoilForce () {
        this.transform
            .DOLocalMoveX (-this.recoilForceDis, this.recoilForceInterval / 2)
            .OnComplete (() => {
                this.transform.DOLocalMoveX (0, this.recoilForceInterval / 2);
            });
    }

    public virtual void launchBullet (float bulletDir) {
        if (this.launchTimer < this.launchInterval) {
            return;
        }

        this.spawnShotGunFire ();

        this.spawnRecoilForce ();

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

    }

    public float getWeaponMpConsume () {
        return this.weaponConfigData.mpConsume;
    }
}