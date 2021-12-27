/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using System.Collections.Generic;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
		// BTNode = new ParallelNode (1).addChild (
		// 	new SelectorNode ().addChild (
		// 		new LanJingGuaiDeadAction (),
		// 		new LanJingGuaiRepelAction (),
		// 		new SequenceNode ().addChild (
		// 			new LanJingGuaiIdleAction (),
		// 			new LanJingGuaiRandomMoveAction (),
		// 			new LanJingGuaiAttackAction ()
		// 		)
		// 	)
		// );
		BTNode = new ParallelNode (1).addChild (
			new SelectorNode ().addChild (
				new SequenceNode ().addChild (
					new NormalDead ().setPreCondition (new IsDead ()),
					new Rebound ()
				),
				new SequenceNode ().addChild (
					new IdleAction ()
				)
			)
		);
	}

	private List<Vector3> pathPosList = new List<Vector3> ();
	public void genRandomTargetPos () {
		// 产生随机移动的目标位置
		Vector3 worldPos = this._pathFinding.getRandomCellPos ();
		this.pathPosList = this._pathFinding.findPath (this.transform.position, worldPos);
		if (this.pathPosList == null) {
			return;
		}

		this.drawPathList = new List<Vector3> (this.pathPosList);

		this.curMoveIndex = 0;
		this.getNextTargetPos ();
	}
