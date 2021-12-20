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

    private GameObject attackEffectNode;

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

        this.checkEffect ();
        this.playAttackAnimation ();
    }

    private void checkEffect () {
        if (this.attackEffectNode != null) {
            return;
        }
        GameObject effectPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (this.weaponConfigData.bulletUrl);
        this.attackEffectNode = ObjectPool.instance.requestInstance (effectPrefab);

        Transform effectTransform = ModuleManager.instance.playerManager.getMeleeEffectTransform ();
        this.attackEffectNode.transform.SetParent (effectTransform);
        // FIXME: 临时逻辑
        this.attackEffectNode.transform.localPosition = new Vector3 (0.4990001f, 0.08199999f, 0);
        this.attackEffectNode.SetActive (false);
    }

    private bool isInAttackState = false;
    private readonly float attackAnimationTime = 0.2f;

    private float animationSpeed;

    private float animationTimer;

    private void playAttackAnimation () {

        // 开启动画，角度从66~-66
        this.animationSpeed = 132 / this.attackAnimationTime;
        this.isInAttackState = true;
        this.animationTimer = 0;

        Transform meleeRotationTransfrom = ModuleManager.instance.playerManager.getMeleeRotationTransform ();
        meleeRotationTransfrom
            .DOLocalRotate (new Vector3 (0, 0, 66), 0.2f)
            .SetEase (Ease.OutCubic)
            .OnComplete (() => {
                meleeRotationTransfrom
                    .DOLocalRotate (new Vector3 (0, 0, -66), this.attackAnimationTime)
                    .SetEase (Ease.OutCubic)
                    .OnComplete (() => {
                        meleeRotationTransfrom
                            .DOLocalRotate (Vector3.zero, this.attackAnimationTime)
                            .SetEase (Ease.OutCubic)
                            .OnComplete (() => {
                                this.isInAttackState = false;
                            });
                    });
            });
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
            this.attackEffectNode.SetActive (false);
            meleeRotationTransfrom.localEulerAngles = Vector3.zero;
            return;
        }

        if (this.animationTimer >= this.attackAnimationTime / 4) {
            if (!this.attackEffectNode.activeSelf) {
                this.attackEffectNode.SetActive (true);
            }
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