/*
 * @Author: l hy 
 * @Date: 2021-10-23 08:46:42 
 * @Description: 子弹数据
 */

using UnityEngine;
public class BulletData {
	public Vector3 bulletDir;

	public float moveSpeed;

	public bool isDie;

	public BulletData (Vector3 bulletDir, float moveSpeed) {
		this.bulletDir = bulletDir;
		this.moveSpeed = moveSpeed;
		this.isDie = false;
	}
}