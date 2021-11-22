/*
 * @Author: l hy 
 * @Date: 2021-11-22 19:36:57 
 * @Description: 自动触发item
 */

using UFramework;
using UnityEngine;

public class AutoTriggerItem : BaseItem {

    protected bool initOver = false;

    public override void localUpdate (float dt) {
        if (!this.initOver) {
            return;
        }

        this.check ();

        this.executeAnimation (dt);
    }

    protected bool isTriggered = false;
    protected float triggerDistance = 3f;

    protected void check () {
        if (this.isTriggered) {
            return;
        }

        float distance = this.getSelfToPlayerDis ();

        if (distance > triggerDistance) {
            return;
        }

        this.isTriggered = true;
        this.isExecuteAnimation = true;
    }

    protected float animationTime = 0.2f;
    protected bool isExecuteAnimation = false;
    protected float curAnimationTimer = 0;

    protected virtual void executeAnimation (float dt) {
        if (!this.isExecuteAnimation) {
            return;
        }
        float distance = this.getSelfToPlayerDis ();
        float speed = distance / (this.animationTime - this.curAnimationTimer);

        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        Vector3 moveDir = (playerPos - this.transform.position).normalized;

        this.transform.position = this.transform.position + moveDir * speed * dt;

        this.curAnimationTimer += dt;

        distance = this.getSelfToPlayerDis ();
        if (distance < 0.15f) {
            this.isExecuteAnimation = false;
            this.animationCompleted ();
        }
    }

    protected virtual void animationCompleted () {

    }

    protected virtual void reset () {
        this.initOver = this.isExecuteAnimation = this.isTriggered = false;

        this.curAnimationTimer = 0;
    }

    protected void OnDisable () {
        this.reset ();
    }
}