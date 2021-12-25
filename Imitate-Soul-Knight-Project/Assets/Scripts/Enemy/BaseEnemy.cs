using System;
using System.Collections.Generic;
using DG.Tweening;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UFramework.Tween;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IAgent {

	protected Animator animator;

	protected BoxCollider2D boxCollider2D;

	protected float intensity = 2.5f;
	protected Material material;

	protected string bulletLayer = LayerGroup.enemyWeapon;

	protected BehaviourTreeRunner behaviourTreeRunner;

	protected virtual void OnEnable () {
		this.animator = this.GetComponent<Animator> ();
		this.boxCollider2D = this.GetComponent<BoxCollider2D> ();
		this.behaviourTreeRunner = this.GetComponent<BehaviourTreeRunner> ();
	}

	protected BlackBoardMemory blackboardMemory;

	protected BaseNode BTNode;

	public Func<bool> isRoomActive;

	protected EnemyConfigData enemyConfigData;

	protected EnemyData enemyData;

	protected PathFinding pathFinding;

	protected float reflectOffset;

	public virtual void init (EnemyConfigData enemyConfigData, Func<bool> isRoomActive) {
		this.enemyConfigData = enemyConfigData;
		this.isRoomActive = isRoomActive;

		this.enemyData = new EnemyData (this.enemyConfigData);
		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer> ();
		this.reflectOffset = Mathf.Max (spriteRenderer.size.x, spriteRenderer.size.y);

		if (this.material == null) {
			this.material = spriteRenderer.material;
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
		behaviourTreeRunner.execute (BTNode, this, blackboardMemory);
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

	private void dissolveDead () {
		this.material
			.DOFloat (0, "_Fade", 2.5f)
			.OnComplete (() => {
				this.recovery ();
			});
	}

	protected List<Vector3> deadPath = new List<Vector3> ();
	protected float deadDistance = 6.5f;
	protected float deadAnimationTime = 2.5f;
	public void deadMove (Vector2 aimDir) {
		this.getDeadPath (aimDir);
		this.getDeadTween ();
	}

	protected void getDeadTween () {
		if (this.deadPath == null || this.deadPath.Count <= 0) {
			return;
		}

		this.transform
			.pathTween (this.deadPath, this.deadAnimationTime)
			.setEase (EaseType.OutCubic)
			.setCompleted (() => {
				ModuleManager.instance.promiseTimer.waitFor (1.0f).then (() => {
					this.dissolveDead ();
				});
			});
	}

	/// <summary>
	/// 获取死亡路径
	/// </summary>
	/// <param name="aimDir"></param>
	protected void getDeadPath (Vector2 aimDir) {
		deadPath.Clear ();
		Vector2 startPos = this.transform.position;
		float leftDistance = deadDistance;
		RaycastHit2D raycastHitInfo;
		while (leftDistance > 0) {
			raycastHitInfo = Physics2D.Raycast (
				startPos,
				aimDir,
				leftDistance,
				1 << LayerMask.NameToLayer (LayerGroup.block) |
				1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));
			Debug.DrawRay (startPos, aimDir, Color.red);
			if (!raycastHitInfo) {
				break;
			}

			// 偏移值，否则可能因为点存在于碰撞体上，造成反射异常
			Vector2 hitPoint = raycastHitInfo.point + aimDir * -this.reflectOffset;
			deadPath.Add (new Vector3 (hitPoint.x, hitPoint.y, 0));

			float distance = (hitPoint - startPos).magnitude;
			leftDistance -= distance;

			startPos = hitPoint;
			aimDir = Vector2.Reflect (aimDir, raycastHitInfo.normal);
		}

		if (leftDistance > 0) {
			Vector2 endPoint = startPos + aimDir * leftDistance;
			deadPath.Add (new Vector3 (endPoint.x, endPoint.y, 0));
		}

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
			this.boxCollider2D.size,
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

	protected virtual void recovery () {
		ObjectPool.instance.returnInstance (this.gameObject);
	}
}