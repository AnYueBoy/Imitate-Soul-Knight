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
        AnimationClip[] animationClips = this.animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < animationClips.Length; i++) {
            AnimationClip clip = animationClips[i];
            if (clip.name == "attack") {
                this.attackAnimationTime = clip.length;
                break;
            }
        }
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
    }

    private bool isInAttackState = false;

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
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