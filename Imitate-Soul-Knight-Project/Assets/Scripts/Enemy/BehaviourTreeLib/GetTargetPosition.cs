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
    protected override void OnEnter () {
        agentInstace = (BaseEnemy) agent;
        Vector3 playerPos = ModuleManager.instance.playerManager.playerTrans.position;
        List<Vector3> pathList = agentInstace.pathFindComp.findPath (agentInstace.transform.position, playerPos);
        this.blackBoardMemory.SetValue ((int) BlackItemEnum.MOVE_PATH, pathList);
    }

    protected override RunningStatus OnExecute () {
        return nodeRunningState = RunningStatus.Success;
    }

    protected override void OnExit () { }

}