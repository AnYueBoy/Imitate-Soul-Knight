/*
 * @Author: l hy 
 * @Date: 2022-01-04 08:54:53 
 * @Description: 获取到玩家的路径
 */

using System.Collections.Generic;
using UFramework;
using UFramework.AI.BehaviourTree;
using UnityEngine;

public class GetTargetPosition : ActionNode {
    BaseEnemy agentInstace;
    protected override void onEnter () {
        agentInstace = (BaseEnemy) agent;
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        List<Vector3> pathList = agentInstace.pathFindComp.findPath (agentInstace.transform.position, playerPos);
        this.blackBoardMemory.setValue (BlackItemEnum.MOVE_PATH, pathList);
    }

    protected override RunningStatus onExecute () {
        return RunningStatus.Success;
    }

    protected override void onExit () {
    }

}