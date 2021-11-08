using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
		BTNode = new ParallelNode (1).addChild (
			new SelectorNode ().addChild (
				new LanJingGuaiIdleAction (),
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
	public void moveToTargetPos () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
		// 步长
		float step = this.enemyConfigData.moveSpeed * dt;

		// 笛卡尔分量
		float horizontalStep = step * this.tempMoveDir.x;
		float verticalStep = step * this.tempMoveDir.y;

		this.transform.position += new Vector3 (horizontalStep, verticalStep, 0);

		float distance = (this.transform.position - targetPos).magnitude;
		if (distance < step) {
			this.transform.position = targetPos;
		}
	}

	private List<Vector3> pathPosList = new List<Vector3> ();
	public void genRandomTargetPos () {
		// 产生随机移动的目标位置
		Vector3 worldPos = this.pathFinding.getRandomCellPos ();
		this.pathPosList = this.pathFinding.findPath (this.transform.position, worldPos);
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