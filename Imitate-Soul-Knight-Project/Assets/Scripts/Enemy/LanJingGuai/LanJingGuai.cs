/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using UFramework.AI.BehaviourTree;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
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