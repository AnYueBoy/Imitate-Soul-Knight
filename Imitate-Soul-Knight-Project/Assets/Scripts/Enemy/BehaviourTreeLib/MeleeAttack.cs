/*
 * @Author: l hy 
 * @Date: 2021-12-29 13:49:30 
 * @Description:近战攻击判定
 */

using UFramework;
using UFramework.AI.BehaviourTree;

public class MeleeAttack : ActionNode {

    private BaseEnemy agentInstance;
    protected override void onEnter () {
        this.agentInstance = (BaseEnemy) agent;
    }

    protected override RunningStatus onExecute () {
        float aimToPlayerDis = agentInstance.aimToPlayerDistance ();
        if (aimToPlayerDis < agentInstance.meleeAttackRange) {
            float damage = this.agentInstance.enemyConfigData.damage;
            ModuleManager.instance.playerManager.injured (damage);
        }
        return RunningStatus.Executing;
    }

    protected override void onExit () { }
}