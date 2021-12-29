/*
 * @Author: l hy 
 * @Date: 2021-12-29 13:49:30 
 * @Description:近战攻击判定
 */

using UFramework.AI.BehaviourTree;

public class MeleeAttack : ActionNode {
    protected override void onEnter () { }

    protected override RunningStatus onExecute () {
        return RunningStatus.Executing;
    }

    protected override void onExit () { }
}