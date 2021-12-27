/*
 * @Author: l hy 
 * @Date: 2021-12-27 10:47:45 
 * @Description: 移动到目标点
 */
using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class MoveToTarget : ActionNode {
    protected override bool onEvaluate () {
        return this.blackBoardMemory.hasValue (BlackItemEnum.MOVE_PATH);
    }

    private float moveSpeed;
    private Transform actionTarget;
    private List<Vector3> movePathList;
    protected override void onEnter () {
        this.moveSpeed = this.blackBoardMemory.getValue<float> (BlackItemEnum.MOVE_SPEED);
        this.actionTarget = this.blackBoardMemory.getValue<Transform> (BlackItemEnum.ACTION_TARGET);
        this.movePathList = this.blackBoardMemory.getValue<List<Vector3>> (BlackItemEnum.MOVE_PATH);

        this.drawPathList = new List<Vector3> (this.movePathList);
        this.curMoveIndex = 0;
        this.getNextTargetPos ();
    }

    protected override RunningStatus onExecute () {
        RunningStatus runningStatus = this.moveActionHandler ();
        this.drawPath (Color.red);
        return runningStatus;
    }

    protected override void onExit () {
        this.blackBoardMemory.delValue (BlackItemEnum.MOVE_SPEED);
        this.blackBoardMemory.delValue (BlackItemEnum.ACTION_TARGET);
        this.blackBoardMemory.delValue (BlackItemEnum.MOVE_PATH);
    }

    private Vector3 curMoveDir;
    private int curMoveIndex;
    private Vector3 targetPos;
    private List<Vector3> drawPathList;

    private RunningStatus moveActionHandler () {
        float dt = this.blackBoardMemory.getValue<float> ((int) BlackItemEnum.DT);
        // 步长
        float step = this.moveSpeed * dt;

        // 笛卡尔分量
        float horizontalStep = step * this.curMoveDir.x;
        float verticalStep = step * this.curMoveDir.y;

        if (this.curMoveDir.x > 0) {
            this.actionTarget.localScale = Vector3.one;
        } else {
            this.actionTarget.localScale = new Vector3 (-1, 1, 1);
        }

        this.actionTarget.position += new Vector3 (horizontalStep, verticalStep, 0);

        float distance = (this.actionTarget.position - targetPos).magnitude;
        if (distance < step) {
            this.actionTarget.position = targetPos;
            this.curMoveIndex++;
            if (this.curMoveIndex >= this.movePathList.Count) {
                return RunningStatus.Success;
            }
            this.getNextTargetPos ();
        }

        return RunningStatus.Executing;

    }

    private void getNextTargetPos () {
        this.targetPos = this.movePathList[this.curMoveIndex];
        this.curMoveDir = (this.targetPos - this.actionTarget.position).normalized;
    }

    private void drawPath (Color color) {
        if (this.drawPathList == null || this.drawPathList.Count <= 0) {
            return;
        }

        if ((this.actionTarget.position - this.drawPathList[0]).magnitude <= 0.4f) {
            this.drawPathList.RemoveAt (0);
        }
        CommonUtil.drawPath (this.drawPathList, color);
        if (this.drawPathList.Count > 0) {
            CommonUtil.drawLine (this.actionTarget.position, this.drawPathList[0], color);
        }
    }
}