/*
 * @Author: l hy 
 * @Date: 2021-11-03 08:30:46 
 * @Description: 随机移动
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class LanJingGuaiRandomMoveAction : ActionNode {

	protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		return true;
	}

	protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		if (lanJingGuai.curMoveIndex <= -1) {
			lanJingGuai.genRandomTargetPos ();
		}
		lanJingGuai.playMoveAni ();
	}

	protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		if (!lanJingGuai.isReachEnd ()) {
			lanJingGuai.moveToTargetPos ();
			return RunningStatus.Executing;
		}

		return RunningStatus.Finished;
	}

	protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.resetRandomMoveState ();
	}
}