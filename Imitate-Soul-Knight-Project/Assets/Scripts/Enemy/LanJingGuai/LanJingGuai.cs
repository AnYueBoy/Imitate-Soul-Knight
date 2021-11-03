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
		float randomValue = this.aimToPlayerDistance ();
		Vector3 moveDir = this.aimToPlayerDir ();
		targetPos = this.transform.position + moveDir * randomValue;
		tempMoveDir = moveDir;
	}

	public void moveToTargetPos () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
		// 步长
		float step = this.enemyConfigData.moveSpeed * dt;

		// 笛卡尔分量
		float horizontalStep = step * this.tempMoveDir.x;
		float verticalStep = step * this.tempMoveDir.y;
		if (horizontalStep > 0) {
			// 水平右方射线检测
		} else {
			// 水平左方射线检测
		}

		if (verticalStep > 0) {
			// 垂直上方射线检测
		} else {
			// 垂直下发射线检测
		}

		this.transform.position += new Vector3 (horizontalStep, verticalStep, 0);

		float distance = (this.transform.position - targetPos).magnitude;
		if (distance < step) {
			this.transform.position = targetPos;
		}
	}

	private bool isRandomMove = false;

	public void genRandomTargetPos () {
		// 产生随机移动的目标位置
	}

	public void randomMove () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);

	}

	public bool isMoveToTarget () {
		return this.transform.position == targetPos;
	}

	public void resetTargetPos () {
		this.targetPos = Vector3.zero;
	}
}