/*
 * @Author: l hy 
 * @Date: 2021-11-19 14:05:16 
 * @Description: 自动宝箱
 */
using DG.Tweening;
using UFramework;
using UnityEngine;
public class AutoChest : BaseChest {

    private readonly float triggerDistance = 0.5f;

    private bool isTriggered = false;

    public override void localUpdate (float dt) {
        this.check ();
    }

    private void check () {
        if (this.isTriggered) {
            return;
        }

        Transform playerTrans = ModuleManager.instance.playerManager.getPlayerTrans ();
        if ((this.transform.position - playerTrans.position).magnitude < triggerDistance) {
            return;
        }

        this.isTriggered = true;

        this.spawnObjects ();
    }

    public void spawnObjects () {
        // TODO: 触发动画并生成奖励物品
        this.left.DOLocalMoveX (-0.43f, 0.5f).SetEase(Ease.OutQuart);
        this.right.DOLocalMoveX (0.43f, 0.5f).SetEase(Ease.OutQuart);

    }
}