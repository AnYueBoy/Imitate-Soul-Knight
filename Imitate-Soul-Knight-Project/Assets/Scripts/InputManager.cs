/*
 * @Author: l hy 
 * @Date: 2021-09-17 15:52:27 
 * @Description: 输入管理
 */

using UnityEngine;

public class InputManager : MonoBehaviour {

	private Vector2 _moveDir = Vector2.zero;

	public Vector2 MoveDir { get => _moveDir; }

	public RectTransform canvasRect;

	public RectTransform moveRocker;

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
		// 只对左半边屏幕进行触摸相应
		if (touch.position.x > Screen.width / 2) {
			return;
		}

		if (touch.phase == TouchPhase.Began) {
			this.touchStart (outResult);
		}

		if (touch.phase == TouchPhase.Moved) {

		}

		if (touch.phase == TouchPhase.Ended) {

		}

	}

	private void touchStart (Vector2 pos) {
		this.moveRocker.localPosition = pos;
	}

	private void touchMove () {

	}

	private void touchEnd () {

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

}