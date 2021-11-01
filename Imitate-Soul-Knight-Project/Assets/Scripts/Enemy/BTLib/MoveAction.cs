/*
 * @Author: l hy 
 * @Date: 2021-11-01 08:47:22 
 * @Description:  
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class MoveAction : ActionNode {
  protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        // 玩家未进入此房间时，角色处于待机状态
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        return lanJingGuai.callback();
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playMoveAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        return RunningStatus.Finished;
    }

}