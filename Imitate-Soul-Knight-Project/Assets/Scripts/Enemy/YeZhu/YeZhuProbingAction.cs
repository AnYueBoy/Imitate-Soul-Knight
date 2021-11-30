/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:40:20 
 * @Description: 野猪试探攻击状态
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class YeZhuProbingAction : ActionNode {
    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playMoveAni ();
        yeZhu.generateProbingPath ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        if (!yeZhu.canExecuteProbing ()) {
            return RunningStatus.Finished;
        }

        yeZhu.executeProbing ();
        yeZhu.moveToTargetPos ();
        if (yeZhu.isReachEnd ()) {
            yeZhu.generateProbingPath ();
        }
        return RunningStatus.Executing;
    }

    protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.resetProbingState ();
    }
}