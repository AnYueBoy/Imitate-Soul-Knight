/*
 * @Author: l hy 
 * @Date: 2021-12-24 14:29:13 
 * @Description: 击退状态
 */

using UFramework.AI.BehaviourTree;

public class LanJingGuaiRepelAction : ActionNode {
    protected override bool OnEvaluate () {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        return lanJingGuai.isInRepel;
    }

    protected override RunningStatus OnExecute () {
        return nodeRunningState = RunningStatus.Executing;
    }
}