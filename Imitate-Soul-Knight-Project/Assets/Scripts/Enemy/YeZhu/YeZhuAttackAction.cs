/*
 * @Author: l hy 
 * @Date: 2021-11-25 13:43:14 
 * @Description: 野猪攻击状态
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class YeZhuAttackAction : ActionNode {
    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playMoveAni ();
        yeZhu.getAimToPlayerPath ();
        Debug.Log ("enter attack state");
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.moveToTargetPos ();
        if (yeZhu.isReachEnd ()) {
            return RunningStatus.Failed;
        }
        return RunningStatus.Executing;
    }

    protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.resetAttackState ();
    }
}