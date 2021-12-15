/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:35:59 
 * @Description: mp
 */

using DG.Tweening;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;
public class MpItem : AutoTriggerItem {

    private readonly float mpRecoveryValue = 3;

    public void init (Vector3 endPos) {
        this.initOver = false;
        ModuleManager.instance.promiseTimer
            .waitFor (0.15f)
            .then (() => {
                this.transform
                    .DOMove (endPos, 0.9f)
                    .SetEase (Ease.OutQuart)
                    .OnComplete (() => {
                        this.initOver = true;
                    })
                    .Play ();
            });

        // 设置相关参数
        this.triggerDistance = 3.5f;
        this.animationTime = 0.2f;
    }

    protected override void animationCompleted () {
        base.animationCompleted ();
        ObjectPool.instance.returnInstance (this.gameObject);
        ModuleManager.instance.playerManager.addMp (this.mpRecoveryValue);
    }
}