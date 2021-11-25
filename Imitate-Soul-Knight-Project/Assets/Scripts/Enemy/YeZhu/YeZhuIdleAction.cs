/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:32:18 
 * @Description: 野猪idle状态
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class YeZhuIdleAction : ActionNode {
    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        // 玩家未进入此房间时，角色处于待机状态
        YeZhu yeZhu = (YeZhu) agent;
        return !yeZhu.isRoomActive ();
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playIdleAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        return RunningStatus.Executing;
    }

}