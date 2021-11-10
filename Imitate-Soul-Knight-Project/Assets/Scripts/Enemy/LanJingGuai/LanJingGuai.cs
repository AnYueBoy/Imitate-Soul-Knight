/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using System.Collections.Generic;
using UFramework;
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

		this.drawPath (Color.red);

	}

	private List<Vector3> drawPathList = new List<Vector3> ();

	private void drawPath (Color color) {
		if (this.drawPathList == null || this.drawPathList.Count <= 0) {
			return;
		}

		if ((this.transform.position - this.drawPathList[0]).magnitude <= 0.4f) {
			this.drawPathList.RemoveAt (0);
		}
		CommonUtil.drawPath (this.drawPathList, color);
		if (this.drawPathList.Count > 0) {
			CommonUtil.drawLine (this.transform.position, this.drawPathList[0], color);
		}
	}

	private List<Vector3> pathPosList = new List<Vector3> ();
	public void genRandomTargetPos () {
		// 产生随机移动的目标位置
		// Vector3 worldPos = this.pathFinding.getRandomCellPos ();
		Vector3 rolePos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
		this.pathPosList = this.pathFinding.findPath (this.transform.position, rolePos);
		// this.pathPosList = this.pathFinding.findPath (this.transform.position, worldPos);
		if (this.pathPosList == null) {
			return;
		}

		this.drawPathList = new List<Vector3> (this.pathPosList);

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
		return this.pathPosList == null || this.curMoveIndex >= this.pathPosList.Count;
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
		ModuleManager.instance.bulletManager.spawnBullet (upPos, upEulerAngles, 1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

		// 下
		Vector3 downEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, new Vector3 (0, 0, -90));
		ModuleManager.instance.bulletManager.spawnBullet (downPos, downEulerAngles, 1, this.bulletTag, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed);

		this.isAttack = true;

	}

	public void resetRandomMoveState () {
		this.curMoveIndex = -1;
	}

	public bool isAttack = false;

	public void resetAttackState () {
		this.isAttack = false;
	}
}