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
					new IdleAction (),
					new SelectorNode ().addChild (
						new SequenceNode ().addChild (
							new GetRandomPosition (),
							new MoveToTarget ()
						),
						new FourAttack ()
					)
				)
			)
		);
	}

}