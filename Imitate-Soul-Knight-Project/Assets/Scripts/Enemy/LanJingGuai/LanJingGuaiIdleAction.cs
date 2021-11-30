using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class LanJingGuaiIdleAction : ActionNode {

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playIdleAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        // 玩家未进入此房间时，角色处于待机状态
        if (!lanJingGuai.isRoomActive () || lanJingGuai.canExecuteIdle ()) {
            lanJingGuai.executeIdle ();
            return RunningStatus.Executing;
        }
        return RunningStatus.Finished;
    }
}