/*
 * @Author: l hy 
 * @Date: 2021-10-23 08:46:42 
 * @Description: 子弹数据
 */

using UnityEngine;
public class BulletData {
	public float moveSpeed;

	public bool isDie;

	public float moveDir;

	public BulletData (float moveDir, float moveSpeed) {
		this.moveDir = moveDir;
		this.moveSpeed = moveSpeed;
		this.isDie = false;
	}
}