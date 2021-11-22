/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:35:59 
 * @Description: mp
 */

using DG.Tweening;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;
public class MpItem : BaseItem {

    private bool isTriggered = false;

    private readonly float triggerDistance = 3f;

    private bool initOver = false;

    public void init (Vector3 endPos) {
        ModuleManager.instance.promiseTimer
            .waitFor (0.15f)
            .then (() => {
                this.transform
                    .DOMove (endPos, 0.9f)
                    .SetEase(Ease.OutQuart)
                    .OnComplete (() => {
                        this.initOver = true;
                    })
                    .Play ();
            });
    }

    public override void localUpdate (float dt) {
        if (!this.initOver) {
            return;
        }

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

    private readonly float animationTime = 0.4f;
    private void executeAnimation () {
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        this.transform
            .DOMove (playerPos, this.animationTime)
            .OnComplete (() => {
                ObjectPool.instance.returnInstance (this.gameObject);
                ModuleManager.instance.playerManager.addMp (ConstValue.mpRecoveryValue);
            })
            .Play ();
    }
}