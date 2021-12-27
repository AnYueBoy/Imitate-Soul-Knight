/*
 * @Author: l hy 
 * @Date: 2021-11-03 08:30:46 
 * @Description: 随机移动
 */

using UFramework.AI.BehaviourTree;

public class LanJingGuaiRandomMoveAction : ActionNode {

	protected override void onEnter () {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.genRandomTargetPos ();
		lanJingGuai.playMoveAni ();
	}

	protected override RunningStatus onExecute () {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		if (!lanJingGuai.isReachEnd ()) {
			lanJingGuai.moveToTargetPos ();
			return RunningStatus.Executing;
		}

		return RunningStatus.Success;
	}

	protected override void onExit () {
		LanJingGuai lanJingGuai = (LanJingGuai) agent;
		lanJingGuai.resetRandomMoveState ();
	}
}