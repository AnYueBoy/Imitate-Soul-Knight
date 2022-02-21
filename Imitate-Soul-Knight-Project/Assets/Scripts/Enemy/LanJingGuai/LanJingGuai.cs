/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:20:41 
 * @Description: 蓝精怪
 */

using UFramework.AI.BehaviourTree;

public class LanJingGuai : BaseEnemy {

	private void Start () {
		blackboardMemory = new BlackBoardMemory ();
		BTNode = new ParallelNode (1).AddChild (
			new SelectorNode ().AddChild (
				new SequenceNode ().AddChild (
					new NormalDead ().SetPreCondition (new IsDead ()),
					new Rebound ()
				),
				new SequenceNode ().AddChild (
					new IdleAction (),
					new SelectorNode ().AddChild (
						new FailureNode (
							new SequenceNode ().AddChild (
								new GetRandomPosition (),
								new MoveToTarget ()
							)),
						new FailureNode (new FourAttack ())
					)
				)
			)
		);
	}

}