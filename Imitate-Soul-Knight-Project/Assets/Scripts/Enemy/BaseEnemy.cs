using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

	protected Animator animator;
	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
	}

	public virtual void localUpdate (float dt) {

	}
}