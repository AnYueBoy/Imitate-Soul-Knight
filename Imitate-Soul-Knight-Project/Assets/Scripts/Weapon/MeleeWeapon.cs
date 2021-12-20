/*
 * @Author: l hy 
 * @Date: 2021-12-16 13:31:08 
 * @Description: 近战武器
 */

using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class MeleeWeapon : BaseWeapon {
    private Animator animator;

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
        if (this.animator != null) {
            return;
        }
        GameObject effectPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (this.weaponConfigData.bulletUrl);
        GameObject effectNode = ObjectPool.instance.requestInstance (effectPrefab);
        this.animator = effectNode.GetComponent<Animator> ();

        Transform effectTransform = ModuleManager.instance.playerManager.getMeleeEffectTransform ();
        effectNode.transform.SetParent (effectTransform);
    }

    private bool isInAttackState = false;
    private float attackAnimationTime;

    private float animationSpeed;

    private float animationTimer;

    private void playAttackAnimation () {
        this.animator.SetTrigger ("Attack");

        AnimatorStateInfo info = this.animator.GetCurrentAnimatorStateInfo (0);
        this.attackAnimationTime = info.length;

        // 开启动画，角度从60~-60
        this.animationSpeed = 120 / this.attackAnimationTime;
        this.isInAttackState = true;
        this.animationTimer = 0;
        this.transform.parent.eulerAngles = new Vector3 (0, 0, -60);
    }

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
        this.checkRayCast ();
    }

    private void executeAnimation () {
        if (!this.isInAttackState) {
            return;
        }

        if (this.animationTimer > this.attackAnimationTime) {
            this.animationTimer = 0;
            this.isInAttackState = false;
            this.transform.parent.eulerAngles = Vector3.zero;
            return;
        }

        float zEulerAngles = -60 + this.attackTimer * this.animationSpeed;

        this.transform.parent.eulerAngles = new Vector3 (0, 0, zEulerAngles);
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