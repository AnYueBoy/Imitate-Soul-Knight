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

    protected override void triggerHandler (RaycastHit2D raycastInfo) {
        base.triggerHandler (raycastInfo);
        LayerMask resultLayer = raycastInfo.collider.gameObject.layer;
        if (resultLayer == LayerMask.NameToLayer (LayerGroup.block)) {
            // 回收子弹
            this.bulletData.isDie = true;
            this.spawnBulletEffect ();
            return;
        }

        if (resultLayer == LayerMask.NameToLayer (LayerGroup.player) && this.bulletData.layer == LayerGroup.enemyBullet) {
            // 回收子弹、对玩家造成伤害
            this.bulletData.isDie = true;
            this.spawnBulletEffect ();
            ModuleManager.instance.playerManager.injured (this.bulletData.damage);
            return;
        }

        if (resultLayer == LayerMask.NameToLayer (LayerGroup.enemyBullet) && this.bulletData.layer == LayerGroup.playerBullet) {
            // 回收子弹、对敌人造成伤害
            this.bulletData.isDie = true;
            this.spawnBulletEffect ();
            BaseEnemy enemy = raycastInfo.collider.GetComponent<BaseEnemy> ();
            enemy.injured (this.bulletData.damage);
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
}