/*
 * @Author: l hy 
 * @Date: 2021-10-23 15:35:54 
 * @Description: {} 
 */
using UnityEngine;

public class Bullet : MonoBehaviour {

	public BulletData bulletData;

	public void init (BulletData bulletData) {
		this.bulletData = bulletData;
	}

	public void localUpdate (float dt) {
		this.move (dt);
	}

	private void OnTriggerEnter2D (Collider2D other) {
		this.triggerHandler ();
	}

	protected void triggerHandler () {
		this.bulletData.isDie = true;
	}

	protected void move (float dt) {
		float moveSpeed = this.bulletData.moveSpeed;
		float moveDir = this.bulletData.moveDir;
		this.transform.Translate (new Vector3 (moveDir * dt * moveSpeed, 0, 0));
	}
}