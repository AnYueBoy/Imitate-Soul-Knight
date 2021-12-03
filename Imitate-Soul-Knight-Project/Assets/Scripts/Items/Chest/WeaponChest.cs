/*
 * @Author: l hy 
 * @Date: 2021-12-03 08:49:11 
 * @Description: 武器箱子
 */
using DG.Tweening;
using UFramework;
using UnityEngine;

public class WeaponChest : PassiveTriggerItem {
    [SerializeField]
    protected Transform left;

    [SerializeField]
    protected Transform right;

    private void OnEnable () {
        this.triggerDistance = 0.85f;
    }

    public override void triggerHandler () {
        base.triggerHandler ();

        // 执行宝箱动画
        this.executeAnimation ();
    }

    private readonly float animationTime = 0.5f;

    private void executeAnimation () {
        // 触发动画
        this.left
            .DOLocalMoveX (-0.43f, this.animationTime)
            .SetEase (Ease.OutQuart)
            .Play ();
        this.right
            .DOLocalMoveX (0.43f, this.animationTime)
            .SetEase (Ease.OutQuart)
            .Play ();

        ModuleManager.instance.promiseTimer.waitFor (this.animationTime / 2)
            .then (() => {
                //生成奖励物品
                this.spawnReward ();
            });
    }

    private void spawnReward () {
        // TODO: 根据配置随机生成
        // 生成枪械奖励
        ModuleManager.instance.itemManager.spawnWeapon (this.transform.position, ItemIdEnum.SNIPER_WEAPON);
    }
}