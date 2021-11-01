using System;
using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IAgent {

	protected Animator animator;
	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
	}

	protected BlackBoardMemory blackboardMemory;

	protected BaseNode BTNode;

	public Func<bool> callback;

	public virtual void init (Func<bool> callback) {
		this.callback = callback;
	}

	public virtual void localUpdate (float dt) {
		BehaviourTreeRunner.execute (BTNode, this, blackboardMemory);
	}
}