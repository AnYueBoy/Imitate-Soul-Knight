/*
 * @Author: l hy 
 * @Date: 2021-11-08 19:42:01 
 * @Description: 随机移动
 */

using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class LanJingGuaiRandomMove : ActionNode {

	protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
		// 玩家进入此房间，且相隔距离超出则移动
		return false;
	}

	protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.playMoveAni ();
	}

	protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.moveToTargetPos ();
		return RunningStatus.Finished;
	}

	protected override void onExit (IAgent agent, BlackBoardMemory workingMemory) {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.resetTargetPos ();
	}
}