using System;
using DG.Tweening;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IAgent {

	protected Animator animator;

	protected BoxCollider2D boxCollider2D;

	protected float intensity = 2.5f;
	protected Material material;

	protected string bulletLayer = LayerGroup.enemyBullet;

	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
		this.boxCollider2D = this.GetComponent<BoxCollider2D> ();
	}

	protected BlackBoardMemory blackboardMemory;

	protected BaseNode BTNode;

	public Func<bool> isRoomActive;

	protected EnemyConfigData enemyConfigData;

	protected EnemyData enemyData;

	protected PathFinding pathFinding;

	public virtual void init (EnemyConfigData enemyConfigData, Func<bool> isRoomActive) {
		this.enemyConfigData = enemyConfigData;
		this.isRoomActive = isRoomActive;

		this.enemyData = new EnemyData (this.enemyConfigData);

		if (this.material == null) {
			this.material = this.GetComponent<SpriteRenderer> ().material;
		}
		this.material.SetFloat ("_Fade", 1);
		float factor = Mathf.Pow (2, intensity);
		Color randomColor = CommonUtil.getRandomColor ();
		randomColor = new Color (randomColor.r * factor, randomColor.g * factor, randomColor.b * factor, randomColor.a * factor);
		this.material.SetColor ("_DissolveColor", randomColor);
	}

	public void setPathFinding (PathFinding pathFinding) {
		this.pathFinding = pathFinding;
	}

	public virtual void localUpdate (float dt) {
		blackboardMemory.setValue ((int) BlackItemEnum.DT, dt);
		BehaviourTreeRunner.execute (BTNode, this, blackboardMemory);
	}

	public bool isInAttackRange () {
		Vector3 subVec = (this.transform.position - ModuleManager.instance.playerManager.getPlayerTrans ().position);
		float distance = subVec.magnitude;
		if (distance <= enemyConfigData.maxAttackDistance && distance >= enemyConfigData.minAttackDistance) {
			return true;
		}
		return false;
	}

	public Vector3 aimToPlayerDir () {
		Vector3 subVec = (ModuleManager.instance.playerManager.getPlayerTrans ().position - this.transform.position);
		return subVec.normalized;
	}

	public float aimToPlayerDistance () {
		Vector3 subVec = (this.transform.position - ModuleManager.instance.playerManager.getPlayerTrans ().position);
		return subVec.magnitude;
	}

	public virtual void injured (float damage) {
		if (!this.isRoomActive ()) {
			return;
		}
		this.enemyData.curHp -= damage;
		this.enemyData.curHp = Mathf.Max (this.enemyData.curHp, 0);
	}

	public bool isDead () {
		return this.enemyData.curHp <= 0;
	}

	public void dissolveDead () {
		this.material
			.DOFloat (0, "_Fade", 2.5f)
			.OnComplete (() => {
				this.recovery ();
			});
	}

	protected virtual void recovery () {
		ObjectPool.instance.returnInstance (this.gameObject);
	}
}