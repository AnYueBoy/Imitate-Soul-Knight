/*
 * @Author: l hy 
 * @Date: 2021-11-01 08:47:22 
 * @Description:  
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class LanJingGuaiMoveAction : ActionNode {
    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        // 玩家进入此房间，且相隔距离超出则移动
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        if (lanJingGuai.targetPos == Vector3.zero && !lanJingGuai.isInAttackRange ()) {
            lanJingGuai.randomAttackDistance ();
            return true;
        }

        if (lanJingGuai.targetPos != Vector3.zero) {
            return !lanJingGuai.isMoveToTarget ();
        }
        return false;
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playMoveAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.moveToTargetPos();
        return RunningStatus.Finished;
    }

    protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.resetTargetPos ();
    }

}