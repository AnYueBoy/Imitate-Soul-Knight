/*
 * @Author: l hy 
 * @Date: 2021-12-27 15:31:48 
 * @Description: 普通死亡状态
 */

using UFramework.AI.BehaviourTree;

public class NormalDead : ActionNode {
    private BaseEnemy agentInstance;
    protected override void OnEnter () {
        this.agentInstance = (BaseEnemy) agent;

        this.agentInstance.playDeadAni ();

        if (agentInstance.boxCollider.enabled) {
            agentInstance.boxCollider.enabled = false;
        }
    }

    protected override RunningStatus OnExecute () {
        return nodeRunningState = RunningStatus.Success;
    }

    protected override void OnExit () { }

}