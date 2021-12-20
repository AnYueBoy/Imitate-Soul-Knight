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
        // FIXME: 临时逻辑
        effectNode.transform.localPosition = new Vector3 (0.4990001f, 0.08199999f, 0);
        effectNode.SetActive (false);
    }

    private bool isInAttackState = false;
    private float attackAnimationTime = -1;

    private float animationSpeed;

    private float animationTimer;

    private void playAttackAnimation () {
        this.animator.gameObject.SetActive (true);
        this.animator.SetTrigger ("Attack");
        this.getAttackAnimationTime ();

        // 开启动画，角度从66~-30
        this.animationSpeed = 96 / this.attackAnimationTime * 2;
        this.isInAttackState = true;
        this.animationTimer = 0;
    }

    private void getAttackAnimationTime () {
        if (this.attackAnimationTime != -1) {
            return;
        }

        AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++) {
            if (clips[i].name == "blueSword") {
                this.attackAnimationTime = clips[i].length;
                break;
            }
        }
    }

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
        this.executeAnimation (dt);
        this.checkRayCast ();
    }

    private void executeAnimation (float dt) {
        if (!this.isInAttackState) {
            return;
        }

        this.animationTimer += dt;

        Transform meleeRotationTransfrom = ModuleManager.instance.playerManager.getMeleeRotationTransform ();

        if (this.animationTimer > this.attackAnimationTime / 2) {
            this.animationTimer = 0;
            this.isInAttackState = false;
            this.animator.gameObject.SetActive (false);
            meleeRotationTransfrom.localEulerAngles = Vector3.zero;
            return;
        }

        float zEulerAngles = 60 - this.attackTimer * this.animationSpeed;

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