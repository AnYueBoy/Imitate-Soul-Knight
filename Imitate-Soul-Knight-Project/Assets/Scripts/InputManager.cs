/*
 * @Author: l hy 
 * @Date: 2021-09-17 15:52:27 
 * @Description: 输入管理
 */

using UnityEngine;

public class InputManager {

	private Vector2 _moveDir = Vector2.zero;

	public Vector2 MoveDir { get => _moveDir; }

	public void localUpdate (float dt) {
		this.editorControl ();
	}

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
}