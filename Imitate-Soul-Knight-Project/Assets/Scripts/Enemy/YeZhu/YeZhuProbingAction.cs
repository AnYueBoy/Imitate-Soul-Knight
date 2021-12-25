/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:40:20 
 * @Description: 野猪试探攻击状态
 */

using UFramework.AI.BehaviourTree;

public class YeZhuProbingAction : ActionNode {
    protected override void onEnter () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playMoveAni ();
        yeZhu.generateProbingPath ();
    }

    protected override RunningStatus onExecute () {
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

    protected override void onExit () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.resetProbingState ();
    }
}