/*
 * @Author: l hy 
 * @Date: 2021-10-16 16:32:33 
 * @Description: 配置管理
 */

using LitJson;
using UFramework.Core;
using UFramework.GameCommon;
using UnityEngine;

public class ConfigManager : IConfigManager {

	private WeaponConfig weaponConfig;

	private EnemyConfig enemyConfig;

	private RoleConfig roleConfig;

	public WeaponConfig WeaponConfig => weaponConfig;

	public EnemyConfig EnemyConfig => enemyConfig;

	public RoleConfig RoleConfig => roleConfig;

	public void Init () {
		weaponConfig = this.loadConfig<WeaponConfig> (ConfigAssetsUrl.weaponConfig);
		enemyConfig = this.loadConfig<EnemyConfig> (ConfigAssetsUrl.enemyConfig);
		roleConfig = this.loadConfig<RoleConfig> (ConfigAssetsUrl.roleConfig);
	}

	private T loadConfig<T> (string configUrl) {
		TextAsset configContext = App.Make<IAssetsManager> ().GetAssetByUrlSync<TextAsset> (configUrl);
		string context = configContext.text;
		T configData = JsonMapper.ToObject<T> (context);
		(configData as IConfig).convertData ();
		return configData;
	}
}