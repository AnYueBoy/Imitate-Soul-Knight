/*
 * @Author: l hy 
 * @Date: 2021-12-27 16:01:12 
 * @Description: 反弹
 */

using System.Collections.Generic;
using DG.Tweening;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.GameCommon;
using UFramework.Tween;
using UnityEngine;

public class Rebound : ActionNode {
    private BaseEnemy agentInstance;
    private Vector2 reboundDir;
    protected override void OnEnter () {
        this.agentInstance = (BaseEnemy) agent;
        // FIXME: 反弹方向未设置
        this.reboundDir = this.blackBoardMemory.GetValue<Vector2> ((int) BlackItemEnum.REBOUND_DIR);

        this.generateReBoundPath ();
        this.reboundTween ();
    }

    protected override RunningStatus OnExecute () {
        return nodeRunningState = RunningStatus.Executing;
    }

    protected override void OnExit () {
        this.blackBoardMemory.DelValue ((int)BlackItemEnum.REBOUND_DIR);
    }

    private List<Vector3> reboundList = new List<Vector3> ();
    private readonly float reboundDis = 6.5f;
    private readonly float reboundAniTime = 2.5f;
    private void generateReBoundPath () {
        Vector2 startPos = this.agentInstance.transform.position;
        float leftDistance = this.reboundDis;
        float reboundOffset = this.agentInstance.reboundOffset;
        RaycastHit2D raycastHitInfo;
        while (leftDistance > 0) {
            raycastHitInfo = Physics2D.Raycast (
                startPos,
                this.reboundDir,
                leftDistance,
                1 << LayerMask.NameToLayer (LayerGroup.block) |
                1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));
            Debug.DrawRay (startPos, this.reboundDir, Color.red);
            if (!raycastHitInfo) {
                break;
            }

            // 偏移值，否则可能因为点存在于碰撞体上，造成反射异常
            Vector2 hitPoint = raycastHitInfo.point + this.reboundDir * -reboundOffset;
            reboundList.Add (new Vector3 (hitPoint.x, hitPoint.y, 0));

            float distance = (hitPoint - startPos).magnitude;
            leftDistance -= distance;

            startPos = hitPoint;
            this.reboundDir = Vector2.Reflect (this.reboundDir, raycastHitInfo.normal);
        }

        if (leftDistance > 0) {
            Vector2 endPoint = startPos + this.reboundDir * leftDistance;
            reboundList.Add (new Vector3 (endPoint.x, endPoint.y, 0));
        }

    }

    private void reboundTween () {
        if (this.reboundList == null || this.reboundList.Count <= 0) {
            return;
        }
        this.agentInstance.transform
            .PathTween (this.reboundList, this.reboundAniTime)
            .SetEase (EaseType.OutCubic)
            .SetCompleted (() => {
                ModuleManager.instance.promiseTimer.waitFor (1.0f).then (() => {
                    this.dissolveDead ();
                });
            });
    }

    private void dissolveDead () {
        this.agentInstance.material
            .DOFloat (0, "_Fade", 2.5f)
            .OnComplete (() => {
                ObjectPool.instance.returnInstance (this.agentInstance.gameObject);
            });
    }
}