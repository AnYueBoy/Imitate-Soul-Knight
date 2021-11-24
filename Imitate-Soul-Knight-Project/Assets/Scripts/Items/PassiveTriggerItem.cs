/*
 * @Author: l hy 
 * @Date: 2021-11-23 19:58:56 
 * @Description: 被动触发item
 */

using UFramework;
using UnityEngine;
public class PassiveTriggerItem : BaseItem {
    public override void localUpdate (float dt) {
        this.check ();
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
    }

    public virtual void triggerHandler () {
        this.isTriggered = true;
        // TODO: 具体的触发逻辑
    }

    protected virtual void reset () {
        this.isTriggered = false;
    }

    protected void OnDisable () {
        this.reset ();
    }
}