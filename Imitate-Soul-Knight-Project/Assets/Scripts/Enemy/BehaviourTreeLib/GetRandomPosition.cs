/*
 * @Author: l hy 
 * @Date: 2021-12-27 16:54:51 
 * @Description: 获取随机位置
 */

using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UnityEngine;

public class GetRandomPosition : ActionNode {

    private BaseEnemy agentInstance;
    protected override void onEnter () {
        this.agentInstance = (BaseEnemy) agent;
    }

    protected override RunningStatus onExecute () {
        this.curNodeRunningStatus = this.generateRandomPathList ();
        return this.curNodeRunningStatus;
    }

    protected override void onExit () { }

    private RunningStatus generateRandomPathList () {
        // 产生随机移动的目标位置
        Vector3 worldPos = this.agentInstance.pathFindComp.getRandomCellPos ();
        List<Vector3> pathList = this.agentInstance.pathFindComp.findPath (this.agentInstance.transform.position, worldPos);
        if (pathList == null) {
            return RunningStatus.Failed;
        }
        this.blackBoardMemory.setValue (BlackItemEnum.MOVE_PATH, pathList);
        this.blackBoardMemory.setValue (BlackItemEnum.CUR_MOVE_SPEED, agentInstance.enemyConfigData.moveSpeed);
        return RunningStatus.Success;
    }

}