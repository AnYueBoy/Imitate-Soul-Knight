/*
 * @Author: l hy 
 * @Date: 2021-12-27 10:47:45 
 * @Description: 移动到目标点
 */
using System.Collections.Generic;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class MoveToTarget : ActionNode {
    protected override bool onEvaluate () {
        return this.blackBoardMemory.hasValue (BlackItemEnum.MOVE_PATH);
    }

    private float moveSpeed;
    private List<Vector3> movePathList;
    private BaseEnemy agentInstance;

    protected override void onEnter () {
        agentInstance = (BaseEnemy) agent;
        agentInstance.playMoveAni ();

        this.moveSpeed = this.blackBoardMemory.getValue<float> (BlackItemEnum.CUR_MOVE_SPEED);

        this.movePathList = this.blackBoardMemory.getValue<List<Vector3>> (BlackItemEnum.MOVE_PATH);

        this.drawPathList = new List<Vector3> (this.movePathList);
        this.curMoveIndex = 0;
        this.getNextTargetPos ();
    }

    protected override RunningStatus onExecute () {
        this.curNodeRunningStatus = this.moveActionHandler ();
        this.drawPath (Color.red);
        return this.curNodeRunningStatus;
    }

    protected override void onExit () {
        this.blackBoardMemory.delValue (BlackItemEnum.MOVE_PATH);
        this.blackBoardMemory.delValue (BlackItemEnum.CUR_MOVE_SPEED);
    }

    private Vector3 curMoveDir;
    private int curMoveIndex;
    private Vector3 targetPos;
    private List<Vector3> drawPathList;

    private RunningStatus moveActionHandler () {
        // 步长
        float step = this.moveSpeed * dt;

        // 笛卡尔分量
        float horizontalStep = step * this.curMoveDir.x;
        float verticalStep = step * this.curMoveDir.y;

        Vector3 toWardsDir = (ModuleManager.instance.playerManager.getPlayerTrans ().position - this.agentInstance.transform.position).normalized;
        if (toWardsDir.x > 0) {
            this.agentInstance.transform.localScale = Vector3.one;
        } else {
            this.agentInstance.transform.localScale = new Vector3 (-1, 1, 1);
        }

        this.agentInstance.transform.position += new Vector3 (horizontalStep, verticalStep, 0);

        float distance = (this.agentInstance.transform.position - targetPos).magnitude;
        if (distance < step) {
            this.agentInstance.transform.position = targetPos;
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
        this.curMoveDir = (this.targetPos - this.agentInstance.transform.position).normalized;
    }

    private void drawPath (Color color) {
        if (this.drawPathList == null || this.drawPathList.Count <= 0) {
            return;
        }

        if ((this.agentInstance.transform.position - this.drawPathList[0]).magnitude <= 0.4f) {
            this.drawPathList.RemoveAt (0);
        }
        CommonUtil.drawPath (this.drawPathList, color);
        if (this.drawPathList.Count > 0) {
            CommonUtil.drawLine (this.agentInstance.transform.position, this.drawPathList[0], color);
        }
    }
}