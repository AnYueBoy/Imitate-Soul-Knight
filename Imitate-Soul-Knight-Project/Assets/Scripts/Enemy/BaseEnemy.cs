using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UnityEngine;
using UFramework.AI.BlackBoard;

public class BaseEnemy : MonoBehaviour, IAgent {

	protected Animator animator;
	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
	}

	protected BlackBoardMemory blackboardMemory;

	protected BaseNode BTNode;

	public virtual void localUpdate (float dt) {
		BehaviourTreeRunner.execute (BTNode, this, blackboardMemory);
	}
}