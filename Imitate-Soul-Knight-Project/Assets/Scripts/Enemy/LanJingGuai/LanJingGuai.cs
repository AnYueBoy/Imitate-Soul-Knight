/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UnityEngine;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
		BTNode = new ParallelNode (1).addChild (
			new SelectorNode ().addChild (
				new LanJingGuaiIdleAction (),
				new LanJingGuaiMoveAction (),
				new LanJingGuaiAttackAction ()
			)
		);
	}

	public void playIdleAni () {
		this.animator.SetBool ("IsMove", false);
	}

	public void playMoveAni () {
		this.animator.SetBool ("IsMove", true);
	}

	public Vector3 targetPos = Vector3.zero;

	protected Vector3 tempMoveDir = Vector3.zero;

	public void randomAttackDistance () {
		float randomValue = CommonUtil.getRandomValue (enemyConfigData.minAttackDistance, enemyConfigData.maxAttackDistance);
		Vector3 moveDir = this.aimToPlayerDir ();
		targetPos = this.transform.position + moveDir * randomValue;
		tempMoveDir = moveDir;
	}

	public void moveToTargetPos () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
		float step = this.enemyConfigData.moveSpeed * dt;
		this.transform.position += step * this.tempMoveDir;

		float distance = (this.transform.position - targetPos).magnitude;
		if (distance < step) {
			this.transform.position = targetPos;
		}
	}

	public bool isMoveToTarget () {
		return this.transform.position == targetPos;
	}

	public void resetTargetPos () {
		this.targetPos = Vector3.zero;
	}
}