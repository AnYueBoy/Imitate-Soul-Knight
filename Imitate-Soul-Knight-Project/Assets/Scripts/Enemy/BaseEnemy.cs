using System;
using DG.Tweening;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

[RequireComponent (typeof (BehaviourTreeRunner))]
public class BaseEnemy : MonoBehaviour, IAgent {

	protected Animator animator;

	protected float intensity = 2.5f;
	protected Material _material;

	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
		this._boxCollider2D = this.GetComponent<BoxCollider2D> ();
		this.behaviourTreeRunner = this.GetComponent<BehaviourTreeRunner> ();
	}

	private BehaviourTreeRunner behaviourTreeRunner;

	protected BlackBoardMemory blackboardMemory;

	protected BaseNode BTNode;

	public Func<bool> isRoomActive;

	protected PathFinding _pathFinding;

	public virtual void init (EnemyConfigData enemyConfigData, Func<bool> isRoomActive) {
		this._enemyConfigData = enemyConfigData;
		this.isRoomActive = isRoomActive;

		this._enemyData = new EnemyData (this.enemyConfigData);
		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer> ();
		this._reboundOffset = Mathf.Max (spriteRenderer.size.x, spriteRenderer.size.y);

		if (this._material == null) {
			this._material = spriteRenderer.material;
		}
		this._material.SetFloat ("_Fade", 1);
		float factor = Mathf.Pow (2, intensity);
		Color randomColor = Util.GetRandomColor ();
		randomColor = new Color (randomColor.r * factor, randomColor.g * factor, randomColor.b * factor, randomColor.a * factor);
		this._material.SetColor ("_DissolveColor", randomColor);
	}

	public void setPathFinding (PathFinding pathFinding) {
		this._pathFinding = pathFinding;
	}

	public virtual void localUpdate (float dt) {
		behaviourTreeRunner.Execute (BTNode, this, blackboardMemory, dt);
	}

	public float aimToPlayerDistance () {
		Vector3 subVec = (this.transform.position - ModuleManager.instance.playerManager.playerTrans.position);
		return subVec.magnitude;
	}

	protected Vector2 damageBulletDir = Vector2.zero;

	public Vector2 getDamageBulletDir () {
		return this.damageBulletDir;
	}

	public virtual void injured (float damage, Vector2 bulletDir) {
		if (!this.isRoomActive ()) {
			return;
		}
		this.enemyData.curHp -= damage;
		this.enemyData.curHp = Mathf.Max (this.enemyData.curHp, 0);
		this.damageBulletDir = bulletDir;
		if (this.enemyData.curHp > 0) {
			// 击退
			this.repel (new Vector3 (bulletDir.x, bulletDir.y, 1));
		}
	}

	public bool isDead () {
		return this.enemyData.curHp <= 0;
	}

	#region  击退状态逻辑
	private float repelDis = 0.5f;
	private float repelTime = 0.2f;
	protected void setRepelInfo (float repelDis, float repelTime) {
		this.repelDis = repelDis;
		this.repelTime = repelTime;
	}

	public bool isInRepel = false;

	protected void repel (Vector3 repelDir) {
		this.isInRepel = true;
		repelDir = repelDir.normalized;
		RaycastHit2D castInfo = Physics2D.BoxCast (
			this.transform.position,
			this._boxCollider2D.size,
			0,
			repelDir,
			this.repelDis,
			1 << LayerMask.NameToLayer (LayerGroup.block) |
			1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));

		if (!castInfo) {
			this.transform
				.DOMove (this.transform.position + repelDir * this.repelDis, this.repelTime)
				.SetEase (Ease.OutCubic)
				.OnComplete (() => {
					this.isInRepel = false;
				});
			return;
		}

		this.transform
			.DOMove (this.transform.position + repelDir * castInfo.distance, this.repelTime)
			.SetEase (Ease.OutCubic)
			.OnComplete (() => {
				this.isInRepel = false;
			});;
	}

	#endregion

	#region  动画状态

	public void playIdleAni () {
		this.animator.SetBool ("IsMove", false);
	}

	public void playMoveAni () {
		this.animator.SetBool ("IsMove", true);
	}

	public void playDeadAni () {
		this.animator.SetBool ("IsDeath", true);
	}
	#endregion

	#region  数据字段
	private readonly float _attackOffset = 0.7f;
	protected readonly float _idleInterval = 1.5f;
	protected float _meleeAttackRange = 0.4f;
	protected string _bulletLayer = LayerGroup.enemyWeapon;
	protected EnemyConfigData _enemyConfigData;
	protected EnemyData _enemyData;
	protected BoxCollider2D _boxCollider2D;
	protected float _reboundOffset;

	#endregion

	#region  数据访问

	public EnemyConfigData enemyConfigData {
		get {
			return this._enemyConfigData;
		}
	}

	public EnemyData enemyData {
		get {
			return this._enemyData;
		}
	}

	public string bulletLayer {
		get {
			return this._bulletLayer;
		}
	}

	public float attackOffset {
		get {
			return this._attackOffset;
		}
	}

	public BoxCollider2D boxCollider {
		get {
			return this._boxCollider2D;
		}
	}

	public float reboundOffset {
		get {
			return this._reboundOffset;
		}
	}

	public Material material {
		get {
			return this._material;
		}
	}

	public float idleInterval {
		get {
			return this._idleInterval;
		}
	}

	public PathFinding pathFindComp {
		get {
			return this._pathFinding;
		}
	}

	public float meleeAttackRange {
		get {
			return this._meleeAttackRange;
		}
	}
	#endregion
}