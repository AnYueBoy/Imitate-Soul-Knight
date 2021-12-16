/*
 * @Author: l hy 
 * @Date: 2021-10-23 15:41:44 
 * @Description: 武器配置数据
 */

public class WeaponConfigData : BaseData {

	public int id; // 唯一标识
	public string name; // 名称
	public string describe; // 描述
	public float attackInterval; // 发射间隔
	public int damage; // 伤害
	public int launchCount; // 发射数量
	public int bulletSpeed; // 子弹速度
	public string bulletUrl; // 子弹路径
	public int mpConsume; // 蓝量消耗值
	public float bulletOffset; // 子弹偏移度
	public float recoilForceDis; // 后坐力作用距离
}