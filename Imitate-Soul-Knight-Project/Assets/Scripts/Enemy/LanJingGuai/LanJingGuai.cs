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
				new LanJingGuaiDeadAction (),
				new LanJingGuaiRepelAction (),
				new SequenceNode ().addChild (
					new LanJingGuaiIdleAction (),
					new LanJingGuaiRandomMoveAction (),
					new LanJingGuaiAttackAction ()
				)
			)
		);
	}

	#region  动画状态
	public void playIdleAni () {
		this.animator.SetBool ("IsMove", false);
	}

	public void playMoveAni () {
		this.animator.SetBool ("IsMove", true);
	}

	public void playerDeadAni () {
		this.animator.SetBool ("IsDeath", true);
	}

	#endregion

	#region  idle状态
	private float idleTimer = 1.5f;

	private readonly float idleInterval = 1.5f;

	public void executeIdle () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
		this.idleTimer += dt;
	}

	public bool canExecuteIdle () {
		return this.idleTimer < idleInterval;
	}

	public void resetIdleState () {
		this.idleTimer = 0;
	}
	#endregion

	#region  移动状态
	private Vector3 targetPos = Vector3.zero;

	private Vector3 tempMoveDir = Vector3.zero;

	private int curMoveIndex = -1;

	public void moveToTargetPos () {
		float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
		// 步长
		float step = this.enemyConfigData.moveSpeed * dt;
		Debug.Log ("moveSpeed: " + this.enemyConfigData.moveSpeed + "step: " + step);

		// 笛卡尔分量
		float horizontalStep = step * this.tempMoveDir.x;
		float verticalStep = step * this.tempMoveDir.y;

		if (this.tempMoveDir.x > 0) {
			this.transform.localScale = Vector3.one;
		} else {
			this.transform.localScale = new Vector3 (-1, 1, 1);
		}

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
		Vector3 worldPos = this.pathFinding.getRandomCellPos ();
		this.pathPosList = this.pathFinding.findPath (this.transform.position, worldPos);
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

	public void resetRandomMoveState () {
		this.curMoveIndex = -1;
	}

	#endregion 

	#region  攻击状态
	private readonly float attackOffset = 0.7f;

	public void attack () {
		Vector3 leftPos = this.transform.position + Vector3.left * this.attackOffset;
		Vector3 rightPos = this.transform.position + Vector3.right * this.attackOffset;
		Vector3 upPos = this.transform.position + Vector3.up * this.attackOffset;
		Vector3 downPos = this.transform.position + Vector3.down * this.attackOffset;

		// 左
		Vector3 leftEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, Vector3.zero);
		ModuleManager.instance.bulletManager.spawnBullet (leftPos, leftEulerAngles, -1, this.bulletLayer, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed, this.enemyConfigData.damage);

		// 右
		Vector3 rightEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, Vector3.zero);
		ModuleManager.instance.bulletManager.spawnBullet (rightPos, rightEulerAngles, 1, this.bulletLayer, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed, this.enemyConfigData.damage);

		// 上
		Vector3 upEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, new Vector3 (0, 0, 90));
		ModuleManager.instance.bulletManager.spawnBullet (upPos, upEulerAngles, 1, this.bulletLayer, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed, this.enemyConfigData.damage);

		// 下
		Vector3 downEulerAngles = CommonUtil.getWorldEulerAngles (this.transform, new Vector3 (0, 0, -90));
		ModuleManager.instance.bulletManager.spawnBullet (downPos, downEulerAngles, 1, this.bulletLayer, this.enemyConfigData.bulletUrl, this.enemyConfigData.bulletSpeed, this.enemyConfigData.damage);

	}

	#endregion

	#region 死亡状态 

	public void invalidCollider () {
		if (this.boxCollider2D.enabled) {
			this.boxCollider2D.enabled = false;
		}
	}

	#endregion
}