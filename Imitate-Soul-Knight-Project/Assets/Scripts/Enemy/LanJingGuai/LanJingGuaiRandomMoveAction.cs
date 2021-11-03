/*
 * @Author: l hy 
 * @Date: 2021-11-03 08:30:46 
 * @Description: 随机移动
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class LanJingGuaiRandomMoveAction : ActionNode {

	protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return false;
	}

	protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.playMoveAni ();
	}

	protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return RunningStatus.Finished;
	}

	protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
	}
}