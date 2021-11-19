/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:39:01 
 * @Description: 普通子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-18 18:30:02
 */

using UFramework;
using UFramework.GameCommon;
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
            this.spawnBulletEffect ();
            return;
        }

        if (this.bulletData.tag == TagGroup.enemyBullet && other.tag == TagGroup.player) {
            // 回收子弹、对玩家造成伤害
            this.bulletData.isDie = true;
            this.spawnBulletEffect ();
            ModuleManager.instance.playerManager.injured (this.bulletData.damage);
            return;
        }

        if (this.bulletData.tag == TagGroup.playerBullet && other.tag == TagGroup.enemy) {
            // 回收子弹、对敌人造成伤害
            this.bulletData.isDie = true;
            this.spawnBulletEffect ();
            BaseEnemy enemy = other.GetComponent<BaseEnemy> ();
            enemy.injured (this.bulletData.damage);
            return;
        }
    }

    private void spawnBulletEffect () {
        GameObject bulletEffectPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (EffectAssetsUrl.normalBulletEffect);
        GameObject bulletEffectNode = ObjectPool.instance.requestInstance (bulletEffectPrefab);
        bulletEffectNode.transform.SetParent (ModuleManager.instance.effectsTrans);
        bulletEffectNode.transform.position = this.transform.position;

        ParticleSystem bulletEffect = bulletEffectNode.GetComponent<ParticleSystem> ();

        ParticleSystem.MainModule mainParticle = bulletEffect.main;
        mainParticle.stopAction = ParticleSystemStopAction.Callback;
    }

    protected override void move (float dt) {
        float moveSpeed = this.bulletData.moveSpeed;
        float moveDir = this.bulletData.moveDir;
        this.transform.Translate (new Vector3 (moveDir * dt * moveSpeed, 0, 0));
    }
}