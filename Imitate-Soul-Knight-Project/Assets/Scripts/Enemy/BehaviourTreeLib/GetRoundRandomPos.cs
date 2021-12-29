/*
 * @Author: l hy 
 * @Date: 2021-12-29 10:38:57 
 * @Description: 获取目标周围随机位置(并产生路径)
 */
using System.Collections.Generic;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class GetRoundRandomPos : ActionNode {
    private YeZhu agentInstance;
    private int probingMinDistance;
    private int probingMaxDistance;

    protected override void onEnter () {
        this.agentInstance = (YeZhu) agent;
        this.probingMinDistance = agentInstance.probingMinDistance;
        this.probingMaxDistance = agentInstance.probingMaxDistance;
    }

    protected override RunningStatus onExecute () {
        return this.generatePathList ();
    }

    protected override void onExit () { }

    private RunningStatus generatePathList () {
        Cell targetCell = this.getTargetCell ();
        if (targetCell == null) {
            return RunningStatus.Failed;
        }

        List<Vector3> pathList = this.agentInstance.pathFindComp.findPath (this.agentInstance.transform.position, targetCell.worldPos);
        if (pathList == null) {
            return RunningStatus.Failed;
        }
        this.blackBoardMemory.setValue (BlackItemEnum.MOVE_PATH, pathList);
        this.blackBoardMemory.setValue (BlackItemEnum.CUR_MOVE_SPEED, agentInstance.enemyConfigData.moveSpeed);
        return RunningStatus.Success;
    }

    private Cell getTargetCell () {
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        int randomProbingDistance = CommonUtil.getRandomValue (this.probingMinDistance, this.probingMaxDistance);

        float curFindCount = 0;
        do {
            int randomX = CommonUtil.getRandomValue (-randomProbingDistance, randomProbingDistance);
            float baseValue = Mathf.Pow (randomProbingDistance, 2) - Mathf.Pow (randomX, 2);
            int intBaseValue = (int) baseValue;
            baseValue = Mathf.Sqrt (baseValue);

            List<Vector3> posList = new List<Vector3> () { new Vector3 (playerPos.x + randomX, playerPos.y + baseValue), new Vector3 (randomX, playerPos.y - baseValue) };
            CommonUtil.confusionElement<Vector3> (posList);

            foreach (Vector3 pos in posList) {
                Cell targetCell = this.agentInstance.pathFindComp.getGridByPos (pos);
                if (targetCell == null) {
                    continue;
                }

                if (targetCell.isObstacle) {
                    continue;
                }

                return targetCell;
            }

            curFindCount++;
        } while (curFindCount < 3);
        return null;
    }
}