/*
 * @Author: l hy 
 * @Date: 2021-12-16 13:31:08 
 * @Description: 近战武器
 */

using System.Collections.Generic;
using UFramework;
using UnityEngine;

public class MeleeWeapon : BaseWeapon {

    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform meleeCheckTrans;

    protected float attackAnimationTime;

    public override void init (ItemIdEnum id) {
        base.init (id);
        this.getAttackAnimationTime ();
    }

    private bool isLaunchRayCast = false;
    private void startRayCast () {
        this.isLaunchRayCast = true;
    }

    private void closeRayCast () {
        this.isLaunchRayCast = false;
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

        this.hitNodeList.Clear ();

        ModuleManager.instance.promiseTimer
            .waitFor (this.attackInterval)
            .then (() => {
                this.isInAttackState = false;
            });

    }

    public override void localUpdate (float dt) {
        this.checkRayCast ();
    }

    private void checkRayCast () {
        if (!this.isLaunchRayCast) {
            return;
        }

        float angle = this.meleeCheckTrans.eulerAngles.z;
        Vector2 weaponSize = this.weaponSpriteRender.size;

        // 当存检测父节点存在碰撞时，检测的数量不一致，临时根据类别区分检测
        int layerMask = 0;
        if (this.weaponLayer == LayerGroup.enemyWeapon) {
            layerMask = 1 << LayerMask.NameToLayer (LayerGroup.player) |
                1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock) |
                1 << LayerMask.NameToLayer (LayerGroup.playerWeapon);
        } else if (this.weaponLayer == LayerGroup.playerWeapon) {
            layerMask = 1 << LayerMask.NameToLayer (LayerGroup.enemy) |
                1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock) |
                1 << LayerMask.NameToLayer (LayerGroup.enemyWeapon);
        } else {
            Debug.LogError ("error layer mask value" + this.weaponLayer);
        }

        RaycastHit2D[] raycastInfo = Physics2D.BoxCastAll (
            this.meleeCheckTrans.position,
            new Vector2 (1.13f, 0.3f),
            angle,
            Vector2.zero,
            0,
            layerMask);

        if (raycastInfo.Length > 0) {
            this.triggerHandler (raycastInfo);
        }
    }

    private List<GameObject> hitNodeList = new List<GameObject> ();
    protected void triggerHandler (RaycastHit2D[] raycastHitInfo) {
        foreach (RaycastHit2D rayCastHit in raycastHitInfo) {
            GameObject hitNode = rayCastHit.collider.gameObject;
            if (hitNodeList.Contains (hitNode)) {
                return;
            }

            hitNodeList.Add (hitNode);
            LayerMask resultLayer = hitNode.layer;
            if (resultLayer == LayerMask.NameToLayer (LayerGroup.enemy) && this.weaponLayer == LayerGroup.playerWeapon) {
                // 武器为玩家且碰撞到了敌人
                // TODO:方向需要计算获得
                hitNode.GetComponent<BaseEnemy> ().injured (this.weaponConfigData.damage, Vector2.down);
                continue;
            }

            if (resultLayer == LayerMask.NameToLayer (LayerGroup.player) && this.weaponLayer == LayerGroup.enemyWeapon) {
                // 武器为敌人且碰撞到玩家
                ModuleManager.instance.playerManager.injured (this.weaponConfigData.damage);
                continue;
            }

            if (resultLayer == LayerMask.NameToLayer (LayerGroup.enemyWeapon) && this.weaponLayer == LayerGroup.playerWeapon) {
                // 武器为玩家且碰撞到敌人子弹
                hitNode.GetComponent<BaseBullet> ().bulletData.isDie = true;
                continue;
            }

            if (resultLayer == LayerMask.NameToLayer (LayerGroup.playerWeapon) && this.weaponLayer == LayerGroup.enemyWeapon) {
                // 武器为敌人且碰撞到玩家子弹
                hitNode.GetComponent<BaseBullet> ().bulletData.isDie = true;
                continue;
            }

            if (resultLayer == LayerMask.NameToLayer (LayerGroup.destructibleBlock)) {
                // 碰撞到可破坏障碍
                hitNode.GetComponent<DestructibleBlock> ().destroyItem ();
                continue;
            }
        }

    }

    private void OnDrawGizmos () {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = this.meleeCheckTrans.localToWorldMatrix;
        Gizmos.DrawWireCube (Vector3.zero, new Vector2 (1.13f, 0.3f));
        Gizmos.matrix = oldMatrix;
    }
}