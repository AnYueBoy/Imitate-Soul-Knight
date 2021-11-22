/*
 * @Author: l hy 
 * @Date: 2021-11-22 19:36:57 
 * @Description: 自动触发item
 */

using DG.Tweening;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class AutoTriggerItem : BaseItem {
    protected bool isTriggered = false;

    protected float triggerDistance = 3f;

    protected bool initOver = false;

    public override void localUpdate (float dt) {
        if (!this.initOver) {
            return;
        }

        this.check ();
    }

    protected void check () {
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

    protected float animationTime = 0.35f;

    protected virtual void executeAnimation () {
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        this.transform
            .DOMove (playerPos, this.animationTime)
            .OnComplete (() => {
                this.animationCompleted ();
            })
            .Play ();
    }

    protected virtual void animationCompleted () {

    }
}