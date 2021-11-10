/*
 * @Author: l hy 
 * @Date: 2021-11-02 08:41:13 
 * @Description: 攻击状态
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class LanJingGuaiAttackAction : ActionNode {
	protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return !lanJingGuai.isAttack;
	}

	protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.playIdleAni ();
		lanJingGuai.attack ();
	}

	protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return RunningStatus.Executing;
	}

	protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.resetAttackState ();
	}
}