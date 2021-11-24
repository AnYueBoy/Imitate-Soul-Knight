/*
 * @Author: l hy 
 * @Date: 2021-09-17 15:52:27 
 * @Description: 输入管理
 */

using System;
using UFramework;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	[SerializeField]
	private ActionButton attackBtn;

	private Vector2 _moveDir = Vector2.zero;

	public Vector2 MoveDir { get => _moveDir; }

	public RectTransform canvasRect;

	public RectTransform moveRocker;

	private Action skillHandler;

	private Action switchHandler;

	private Action interactiveHandler;

	private void OnEnable () {
		this.attackBtn.registerPressed (() => {
			this.triggerAttack ();
		});
	}

	public void localUpdate (float dt) {
		this.editorControl ();
		this.mobileControl ();
	}

	#region  移动端触摸逻辑
	private void mobileControl () {
		if (Input.touchCount <= 0) {
			return;
		}

		Touch touch = Input.touches[0];

		Vector2 outResult;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (this.canvasRect, touch.position, null, out outResult);

		// 对屏幕三分之一出的屏幕进行触摸响应
		if (touch.position.x > Screen.width / 3 && this.touchStartPos == Vector2.zero) {
			this.touchEnd ();
			return;
		}

		if (touch.phase == TouchPhase.Began) {
			this.touchStart (outResult);
		}

		if (touch.phase == TouchPhase.Moved) {
			this.touchMove (outResult);
		}

		if (touch.phase == TouchPhase.Ended) {
			this.touchEnd ();
		}
	}

	private Vector2 touchStartPos;

	private void touchStart (Vector2 pos) {
		this.moveRocker.localPosition = pos;
		this.touchStartPos = pos;
	}

	private readonly float moveMaxDis = 100f;

	public RectTransform movePointer;
	private void touchMove (Vector2 pos) {
		if (this.touchStartPos == Vector2.zero) {
			return;
		}
		Vector2 moveVec = pos - touchStartPos;
		this._moveDir = moveVec.normalized;
		if (moveVec.magnitude > moveMaxDis) {
			this.movePointer.localPosition = this._moveDir * moveMaxDis;
		} else {
			this.movePointer.localPosition = moveVec;
		}
	}

	private void touchEnd () {
		this.touchStartPos = Vector2.zero;
		this._moveDir = Vector2.zero;
		this.movePointer.localPosition = Vector2.zero;
	}

	#endregion

	#region  编辑器输入逻辑
	private void editorControl () {
#if UNITY_EDITOR
		if (Input.GetKey (KeyCode.D)) {
			this._moveDir = Vector2.right;
		} else if (Input.GetKey (KeyCode.A)) {
			this._moveDir = Vector2.left;
		} else if (Input.GetKey (KeyCode.W)) {
			this._moveDir = Vector2.up;
		} else if (Input.GetKey (KeyCode.S)) {
			this._moveDir = Vector2.down;
		}

		if (Input.GetKeyUp (KeyCode.D)) {
			this._moveDir = Vector2.zero;
		} else if (Input.GetKeyUp (KeyCode.A)) {
			this._moveDir = Vector2.zero;
		} else if (Input.GetKeyUp (KeyCode.W)) {
			this._moveDir = Vector2.zero;
		} else if (Input.GetKeyUp (KeyCode.S)) {
			this._moveDir = Vector2.zero;
		}
#endif
	}

	#endregion

	#region  按钮触发
	public void triggerSkill () {
		ModuleManager.instance.playerManager.triggerSkill ();
	}

	public void triggerSwitchWeapon () {
		ModuleManager.instance.playerManager.triggerSwitchWeapon ();
	}

	public void triggerInteractive () {
		ModuleManager.instance.playerManager.triggerInterface ();
	}

	public void triggerAttack () {
		ModuleManager.instance.playerManager.triggerAttack ();
	}
	#endregion

}