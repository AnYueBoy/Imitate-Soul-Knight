using UFramework.AI.BehaviourTree;

public class LanJingGuaiIdleAction : ActionNode {

    protected override void onEnter () {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playIdleAni ();
    }

    protected override RunningStatus onExecute () {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        // 玩家未进入此房间时，角色处于待机状态
        if (!lanJingGuai.isRoomActive () || lanJingGuai.canExecuteIdle ()) {
            lanJingGuai.executeIdle ();
            return RunningStatus.Executing;
        }
        return RunningStatus.Finished;
    }
}