using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-10-23 15:43:03 
 * @Description: 武器配置
 */
public class WeaponConfig  {

	public List<WeaponConfigData> weaponList = new List<WeaponConfigData> ();

	private Dictionary<int, WeaponConfigData> weaponDic = new Dictionary<int, WeaponConfigData> ();

}