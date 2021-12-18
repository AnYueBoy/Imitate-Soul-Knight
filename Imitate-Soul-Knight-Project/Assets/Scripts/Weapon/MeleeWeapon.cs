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

        // TODO: 攻击伤害逻辑

    }

    private void checkEffect () {
        if (this.animator != null) {
            return;
        }
        GameObject effectPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (this.weaponConfigData.bulletUrl);
        GameObject effectNode = ObjectPool.instance.requestInstance (effectPrefab);
        this.animator = effectNode.GetComponent<Animator> ();
    }

    private bool isInAttackState = false;

    private void playAttackAnimation () {
        this.animator.SetTrigger ("Attack");
    }

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
        this.checkAttackState ();
        this.checkRayCast ();
    }

    private void checkAttackState () {
        if (!this.isInAttackState) {
            return;
        }

        AnimatorStateInfo info = this.animator.GetCurrentAnimatorStateInfo (0);
        this.isInAttackState = info.IsName ("blueSword");
    }

    private void checkRayCast () {
        if (!this.isInAttackState) {
            return;
        }
        //TODO: box ray check
    }
}