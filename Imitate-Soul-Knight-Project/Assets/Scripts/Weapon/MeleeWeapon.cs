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

    [SerializeField] protected Animator animator;

    protected float attackAnimationTime;

    public override void init (ItemIdEnum id) {
        base.init (id);
        this.getAttackAnimationTime ();
    }

    protected void getAttackAnimationTime () {
        AnimationClip[] animationClips = this.animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < animationClips.Length; i++) {
            AnimationClip clip = animationClips[i];
            if (clip.name == "Attack") {
                // 僵直等待时间
                this.attackAnimationTime = clip.length + 0.05f;
                break;
            }
        }

        this.attackInterval = Mathf.Max (this.attackAnimationTime, this.attackInterval);
    }

    public override void attack (float args) {
        if (this.isInAttackState) {
            return;
        }

        float weaponConsumeValue = this.weaponConfigData.mpConsume;
        float curMp = ModuleManager.instance.playerManager.getCurMp ();
        if (curMp < weaponConsumeValue) {
            return;
        }

        ModuleManager.instance.playerManager.consumeMp (weaponConsumeValue);

        this.isInAttackState = true;
        this.animator.SetTrigger ("Attack");

        ModuleManager.instance.promiseTimer
            .waitFor (this.attackInterval)
            .then (() => {
                this.isInAttackState = false;
            });

    }

    private bool isInAttackState = false;

    public override void localUpdate (float dt) {
        this.checkRayCast ();
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