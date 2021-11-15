/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:39:01 
 * @Description: 普通子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-15 18:24:43
 */

using UFramework;
using UnityEngine;

public class NormalBullet : BaseBullet {
    private Vector2 moveDir;

    protected override void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == TagGroup.enemyBullet || other.tag == TagGroup.playerBullet) {
            return;
        }

        if (other.tag == TagGroup.block) {
            // 回收子弹
            this.bulletData.isDie = true;
            return;
        }

        if (this.bulletData.tag == TagGroup.enemyBullet && other.tag == TagGroup.player) {
            // 回收子弹、对玩家造成伤害
            this.bulletData.isDie = true;
            ModuleManager.instance.playerManager.injured (this.bulletData.damage);
            return;
        }

        if (this.bulletData.tag == TagGroup.playerBullet && other.tag == TagGroup.enemy) {
            // 回收子弹、对敌人造成伤害
            this.bulletData.isDie = true;
            return;
        }
    }

    protected override void move (float dt) {
        float moveSpeed = this.bulletData.moveSpeed;
        float moveDir = this.bulletData.moveDir;
        this.transform.Translate (new Vector3 (moveDir * dt * moveSpeed, 0, 0));
    }
}