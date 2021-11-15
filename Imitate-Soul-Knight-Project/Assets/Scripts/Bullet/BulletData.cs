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

	public string tag;

	public BulletData (float moveDir, float moveSpeed, float bulletDamgage, string tag) {
		this.moveDir = moveDir;
		this.moveSpeed = moveSpeed;
		this.damage = bulletDamgage;
		this.isDie = false;
		this.tag = tag;
	}
}