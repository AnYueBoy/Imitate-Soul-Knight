using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-12-16 13:31:08 
 * @Description: 近战武器
 */

using DG.Tweening;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class MeleeWeapon : BaseWeapon {

    private List<Sprite> effectSpriteList;

    public override void init (ItemIdEnum id) {
        base.init (id);

        // 加载近战武器的效果图片
        effectSpriteList = AssetsManager.instance.getAllAssetsByUrlSync<Sprite> ("Effect/" + (int) id);
    }

    public override void attack (float args) {
        if (this.isInAttackState) {
            return;
        }

        if (this.attackTimer < this.attackInterval) {
            return;
        }

        float weaponConsumeValue = this.weaponConfigData.mpConsume;
        float curMp = ModuleManager.instance.playerManager.getCurMp ();
        if (curMp < weaponConsumeValue) {
            return;
        }

        ModuleManager.instance.playerManager.consumeMp (weaponConsumeValue);

        this.attackTimer = 0;

        this.playAttackAnimation ();
    }

    private bool isInAttackState = false;
    private readonly float attackAnimationTime = 0.2f;

    private float animationSpeed;

    private float animationTimer;

    private readonly float leftAngle = 66;

    private readonly float rightAngle = -66;

    private void playAttackAnimation () {

        // 开启动画
        this.animationSpeed = (leftAngle - rightAngle) / this.attackAnimationTime;
        this.isInAttackState = true;
        this.animationTimer = 0;

        Transform meleeParentTransform = ModuleManager.instance.playerManager.getMeleeParentTransform ();
        meleeParentTransform.position = new Vector3 (0.638f, 0, 0);
    }

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
        // this.executeAnimation (dt);
        this.checkRayCast ();
    }

    private void executeAnimation (float dt) {
        if (!this.isInAttackState) {
            return;
        }

        this.animationTimer += dt;

        Transform meleeRotationTransfrom = ModuleManager.instance.playerManager.getMeleeRotationTransform ();

        if (this.animationTimer > this.attackAnimationTime) {
            this.animationTimer = 0;
            this.isInAttackState = false;
            meleeRotationTransfrom.localEulerAngles = new Vector3 (0, 0, 30);
            return;
        }

        SpriteRenderer effectSpriteRender = ModuleManager.instance.playerManager.getEffectSprite ();
        if (meleeRotationTransfrom.localEulerAngles.z <= -34 && effectSpriteRender.sprite != this.effectSpriteList[0]) {
            // 显示第一帧刀光
            effectSpriteRender.sprite = this.effectSpriteList[0];
        }

        float zEulerAngles = 66 - this.attackTimer * this.animationSpeed;

        meleeRotationTransfrom.localEulerAngles = new Vector3 (0, 0, zEulerAngles);
    }

    private void checkRayCast () {
        if (!this.isInAttackState) {
            return;
        }
        //TODO: box ray check
        // RaycastHit2D raycastInfo = Physics2D.BoxCast (
        //     this.transform.position,
        //     size,
        //     angle,
        //     checkDir,
        //     step,
        //     1 << LayerMask.NameToLayer (LayerGroup.enemy) |
        //     1 << LayerMask.NameToLayer (LayerGroup.player) |
        //     1 << LayerMask.NameToLayer (LayerGroup.block) |
        //     1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));

        // return raycastInfo;
    }
}