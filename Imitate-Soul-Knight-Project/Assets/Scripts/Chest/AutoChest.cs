/*
 * @Author: l hy 
 * @Date: 2021-11-19 14:05:16 
 * @Description: 自动宝箱
 */
using DG.Tweening;
using UFramework;
using UnityEngine;
public class AutoChest : BaseChest {

    private readonly float triggerDistance = 0.85f;

    private bool isTriggered = false;

    public override void localUpdate (float dt) {
        this.check ();
    }

    private void check () {
        if (this.isTriggered) {
            return;
        }

        Transform playerTrans = ModuleManager.instance.playerManager.getPlayerTrans ();
        float distance = (this.transform.position - playerTrans.position).magnitude;

        if (distance > triggerDistance) {
            return;
        }

        this.isTriggered = true;

        this.spawnObjects ();
    }

    private readonly float animationTime = 0.5f;

    public void spawnObjects () {
        // 触发动画并生成奖励物品
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
                Debug.Log ("生成奖励");
            });
    }
}