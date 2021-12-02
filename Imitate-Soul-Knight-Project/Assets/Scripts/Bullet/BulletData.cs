/*
 * @Author: l hy 
 * @Date: 2021-10-23 08:46:42 
 * @Description: 子弹数据
 */

public class BulletData {
	public float moveSpeed;

	public bool isDie;

	public float moveDir;

	public float damage;

	public string layer;

	public BulletData (float moveDir, float moveSpeed, float bulletDamgage, string layer) {
		this.moveDir = moveDir;
		this.moveSpeed = moveSpeed;
		this.damage = bulletDamgage;
		this.isDie = false;
		this.layer = layer;
	}
}