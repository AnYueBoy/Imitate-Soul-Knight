/*
 * @Author: l hy 
 * @Date: 2021-12-16 13:12:52 
 * @Description: 远程武器
 */

using DG.Tweening;
using UFramework;
using UFramework.FrameUtil;
using UnityEngine;

public class RangeWeapon : BaseWeapon {
    [SerializeField] private Transform launchTrans;

    [SerializeField] protected SpriteRenderer shotGunEffect;

    private readonly float shotGunInterval = 0.2f;

    #region  后坐力
    protected float recoilForceDis;

    /// <summary>
    ///后坐力作用时间 
    /// </summary>
    protected readonly float recoilForceInterval = 0.3f;
    #endregion

    #region  发射
    protected float launchInterval = 0;

    protected float launchTimer = 0;

    #endregion

    public override void init (ItemIdEnum id) {
        base.init (id);

        this.launchInterval = weaponConfigData.launchInterval;
        this.launchTimer = this.launchInterval;

        this.shotGunEffect.color = new Color (1, 1, 1, 0);

        this.recoilForceDis = this.weaponConfigData.recoilForceDis;
    }

    public override void localUpdate (float dt) {
        this.launchTimer += dt;
    }

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

        float weaponConsumeValue = this.weaponConfigData.mpConsume;
        float curMp = ModuleManager.instance.playerManager.getCurMp ();
        if (curMp < weaponConsumeValue) {
            return;
        }

        // 生成枪口火花
        this.spawnShotGunFire ();

        // 生成后坐力
        this.spawnRecoilForce ();

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
            this.weaponLayer,
            this.weaponConfigData.bulletUrl,
            bulletSpeed,
            this.weaponConfigData.damage);
    }

    public override void attack (float args) {
        this.launchBullet (args);
    }
}