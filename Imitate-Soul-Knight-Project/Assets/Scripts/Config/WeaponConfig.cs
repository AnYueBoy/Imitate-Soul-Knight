/*
 * @Author: l hy 
 * @Date: 2021-10-23 15:43:03 
 * @Description: 武器配置
 */
using System.Collections.Generic;
public class WeaponConfig : IConfig {

	public List<WeaponConfigData> weaponList = new List<WeaponConfigData> ();

	private Dictionary<int, WeaponConfigData> weaponDic = new Dictionary<int, WeaponConfigData> ();

	public void convertData () {
		foreach (WeaponConfigData weaponConfigData in weaponList) {
			weaponDic.Add (weaponConfigData.id, weaponConfigData);
		}
	}

	public WeaponConfigData getWeaponConfigDataById (ItemIdEnum id) {
		return this.weaponDic[(int)id];
	}
}