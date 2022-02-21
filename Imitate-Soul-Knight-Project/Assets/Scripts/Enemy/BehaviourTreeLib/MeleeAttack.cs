/*
 * @Author: l hy 
 * @Date: 2021-12-29 13:49:30 
 * @Description:近战攻击判定
 */

using UFramework;
using UFramework.AI.BehaviourTree;

public class MeleeAttack : ActionNode {

    private BaseEnemy agentInstance;
    protected override void OnEnter () {
        this.agentInstance = (BaseEnemy) agent;
    }

    protected override RunningStatus OnExecute () {
        float aimToPlayerDis = agentInstance.aimToPlayerDistance ();
        if (aimToPlayerDis < agentInstance.meleeAttackRange) {
            float damage = this.agentInstance.enemyConfigData.damage;
            ModuleManager.instance.playerManager.injured (damage);
        }
        return nodeRunningState = RunningStatus.Executing;
    }

    protected override void OnExit () { }
}