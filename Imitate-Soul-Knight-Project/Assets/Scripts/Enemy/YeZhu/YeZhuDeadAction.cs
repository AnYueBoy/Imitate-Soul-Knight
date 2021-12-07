/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:24:35 
 * @Description: 野猪死亡状态
 */

using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UnityEngine;

public class YeZhuDeadAction : ActionNode {
    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        return yeZhu.isDead ();
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playerDeadAni ();
        yeZhu.invalidCollider ();

        float randomX = CommonUtil.getRandomValue (-1.0f, 1.0f);
        float randomY = CommonUtil.getRandomValue (-1.0f, 1.0f);
        Vector2 randomAimDir = new Vector2 (randomX, randomY);
        randomAimDir = randomAimDir.normalized;
        yeZhu.deadMove (randomAimDir);
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        return RunningStatus.Executing;
    }
}