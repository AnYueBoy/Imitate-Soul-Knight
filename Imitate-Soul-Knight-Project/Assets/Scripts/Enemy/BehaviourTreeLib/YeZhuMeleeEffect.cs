/*
 * @Author: l hy 
 * @Date: 2022-01-04 08:43:46 
 * @Description: 野猪近战攻击特效
 */

using UFramework.AI.BehaviourTree;

public class YeZhuMeleeEffect : ActionNode {

    YeZhu agentInstace;
    protected override void onEnter () {
        agentInstace = (YeZhu) agent;
        agentInstace.sprintEffectNode.SetActive (true);
    }

    protected override RunningStatus onExecute () {
        return RunningStatus.Executing;
    }

    protected override void onExit () {
        agentInstace.sprintEffectNode.SetActive (false);
    }

}