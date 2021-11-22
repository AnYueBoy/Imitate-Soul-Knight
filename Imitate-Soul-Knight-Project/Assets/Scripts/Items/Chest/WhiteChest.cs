/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:44:51 
 * @Description: 白色宝箱
 */

using DG.Tweening;
using UFramework;
using UFramework.FrameUtil;
using UnityEngine;
public class WhiteChest : BaseItem {
    [SerializeField]
    protected Transform left;

    [SerializeField]
    protected Transform right;
    private readonly float triggerDistance = 0.85f;

    private bool isTriggered = false;

    public override void localUpdate (float dt) {
        this.check ();
    }

    private void check () {
        if (this.isTriggered) {
            return;
        }

        float distance = this.getSelfToPlayerDis ();

        if (distance > triggerDistance) {
            return;
        }

        this.isTriggered = true;

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
        // TODO:根据配置生成奖励物品

        // TODO: 根据配置进行随机

        int randomValue = CommonUtil.getRandomValue (2, 4);
        for (int i = 0; i < randomValue; i++) {
            Vector3 randomPos = CommonUtil.getCircleRandomPos (this.transform.position, 1);
            ModuleManager.instance.itemManager.spawnItem (randomPos, ItemIdEnum.MP_ITEM);
        }
    }
}