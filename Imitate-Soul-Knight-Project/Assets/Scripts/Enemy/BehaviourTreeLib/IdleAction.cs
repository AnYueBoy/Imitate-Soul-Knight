/*
 * @Author: l hy 
 * @Date: 2021-12-27 16:35:00 
 * @Description: 闲置状态
 */

using UFramework.AI.BehaviourTree;

public class IdleAction : ActionNode {

    private BaseEnemy agentInstace;
    protected override void OnEnter () {
        this.agentInstace = (BaseEnemy) agent;
        this.agentInstace.playIdleAni ();
    }

    private float idleTimer;

    protected override RunningStatus OnExecute () {
        if (!agentInstace.isRoomActive ()) {
            return nodeRunningState = RunningStatus.Executing;
        }

        if (this.idleTimer < agentInstace.idleInterval) {
            this.idleTimer += deltaTime;
            return nodeRunningState = RunningStatus.Executing;
        }
        return nodeRunningState = RunningStatus.Success;
    }

    protected override void OnExit () {
        this.idleTimer = 0;
    }

}