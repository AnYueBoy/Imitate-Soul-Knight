/*
 * @Author: l hy 
 * @Date: 2022-01-04 08:43:46 
 * @Description: 野猪近战攻击特效
 */

using UFramework.AI.BehaviourTree;

public class YeZhuMeleeEffect : ActionNode {

    YeZhu agentInstace;
    protected override void OnEnter () {
        agentInstace = (YeZhu) agent;
        agentInstace.sprintEffectNode.SetActive (true);
    }

    protected override RunningStatus OnExecute () {
        return nodeRunningState = RunningStatus.Executing;
    }

    protected override void OnExit () {
        agentInstace.sprintEffectNode.SetActive (false);
    }

}