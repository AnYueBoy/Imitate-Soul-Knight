/*
 * @Author: l hy 
 * @Date: 2021-11-01 08:47:22 
 * @Description:  
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class LanJingGuaiMoveAction : ActionNode {
    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        //TODO: 玩家进入此房间，且相隔距离超出则移动
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        return true;
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playMoveAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;

        return RunningStatus.Finished;
    }

}