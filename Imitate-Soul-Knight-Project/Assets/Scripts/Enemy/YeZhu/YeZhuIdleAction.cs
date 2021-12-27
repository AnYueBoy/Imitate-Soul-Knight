/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:32:18 
 * @Description: 野猪idle状态
 */

using UFramework.AI.BehaviourTree;

public class YeZhuIdleAction : ActionNode {
    protected override void onEnter () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playIdleAni ();
    }

    protected override RunningStatus onExecute () {
        YeZhu yeZhu = (YeZhu) agent;
        if (!yeZhu.isRoomActive ()) {
            return RunningStatus.Executing;
        }
        return RunningStatus.Success;
    }

}