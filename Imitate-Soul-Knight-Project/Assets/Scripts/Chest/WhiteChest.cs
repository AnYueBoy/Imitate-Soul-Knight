/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:44:51 
 * @Description: 白色宝箱
 */

using DG.Tweening;
using UFramework;
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