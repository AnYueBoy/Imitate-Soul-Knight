/*
 * @Author: l hy 
 * @Date: 2021-12-27 16:35:00 
 * @Description: 闲置状态
 */

using UFramework.AI.BehaviourTree;

public class IdleAction : ActionNode {

    private BaseEnemy agentInstace;
    protected override void onEnter () {
        this.agentInstace = (BaseEnemy) agent;
        this.agentInstace.playIdleAni ();
    }

    private float idleTimer;

    protected override RunningStatus onExecute () {
        if (!agentInstace.isRoomActive ()) {
            return RunningStatus.Executing;
        }

        if (this.idleTimer < agentInstace.idleInterval) {
            this.idleTimer += dt;
            return RunningStatus.Executing;
        }
        return RunningStatus.Success;
    }

    protected override void onExit () {
        this.idleTimer = 0;
    }

}