using System.Collections.Generic;
using UFramework;
/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UnityEngine;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
		BTNode = new ParallelNode (1).addChild (
			new SelectorNode ().addChild (
				new LanJingGuaiIdleAction (),
				new LanJingGuaiRandomMoveAction (),
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

	private Vector3 targetPos = Vector3.zero;

	private Vector3 tempMoveDir = Vector3.zero;

	public int curMoveIndex = -1;

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
			this.curMoveIndex++;
			this.getNextTargetPos ();
		}
	}

	private List<Vector3> pathPosList = new List<Vector3> ();
	public void genRandomTargetPos () {
		// 产生随机移动的目标位置
		Vector3 worldPos = this.pathFinding.getRandomCellPos ();
		this.pathPosList = this.pathFinding.findPath (this.transform.position, worldPos);

		this.curMoveIndex = 0;
		this.getNextTargetPos ();
	}

	private void getNextTargetPos () {
		if (this.curMoveIndex >= this.pathPosList.Count) {
			return;
		}
		this.targetPos = this.pathPosList[this.curMoveIndex];
		this.tempMoveDir = (this.targetPos - this.transform.position).normalized;
	}

	public bool isReachEnd () {
		return this.curMoveIndex >= this.pathPosList.Count;
	}

	private readonly float attackOffset = 0.7f;

	public void attack () {
		Vector3 leftPos = this.transform.position + Vector3.left * this.attackOffset;
		Vector3 rightPos = this.transform.position + Vector3.right * this.attackOffset;
		Vector3 upPos = this.transform.position + Vector3.up * this.attackOffset;
		Vector3 downPos = this.transform.position + Vector3.down * this.attackOffset;

		// 左
		Vector3 leftEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, Vector3.zero);
		ModuleManager.instance.bulletManager.spawnBullet (leftPos, leftEulerAngles, -1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

		// 右
		Vector3 rightEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, Vector3.zero);
		ModuleManager.instance.bulletManager.spawnBullet (rightPos, rightEulerAngles, 1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

		// 上
		Vector3 upEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, new Vector3 (0, 0, 90));
		ModuleManager.instance.bulletManager.spawnBullet (upPos, Vector3.zero, 1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

		// 下
		Vector3 downEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, new Vector3 (0, 0, -90));
		ModuleManager.instance.bulletManager.spawnBullet (downPos, Vector3.zero, 1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

	}

	public void resetTargetPos () {
		this.curMoveIndex = -1;
	}
}