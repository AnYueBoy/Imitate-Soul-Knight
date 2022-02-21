/*
 * @Author: l hy 
 * @Date: 2021-12-27 15:33:45 
 * @Description: 是否死亡
 */
using UFramework.AI.BehaviourTree;
public class IsDead : BaseCondition {
    public override bool IsTrue (IAgent agent) {
        BaseEnemy agentInstance = (BaseEnemy) agent;
        return agentInstance.enemyData.curHp <= 0;
    }
}