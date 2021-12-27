/*
 * @Author: l hy 
 * @Date: 2021-11-02 08:41:13 
 * @Description: 攻击状态
 */

using UFramework.AI.BehaviourTree;

public class LanJingGuaiAttackAction : ActionNode {
	protected override void onEnter () {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.playIdleAni ();
		lanJingGuai.attack ();
		lanJingGuai.resetIdleState ();
	}

	protected override RunningStatus onExecute () {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return RunningStatus.Success;
	}
}