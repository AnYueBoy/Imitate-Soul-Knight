/*
 * @Author: l hy 
 * @Date: 2021-10-16 16:32:33 
 * @Description: 配置管理
 */

using LitJson;
using UFramework.GameCommon;
using UnityEngine;

public class ConfigManager {

	public LevelConfig levelConfig;

	public WeaponConfig weaponConfig;

	public EnemyConfig enemyConfig;

	public RoleConfig roleConfig;

	public void init () {
		levelConfig = this.loadConfig<LevelConfig> (ConfigAssetsUrl.levelConfig);
		weaponConfig = this.loadConfig<WeaponConfig> (ConfigAssetsUrl.weaponConfig);
		enemyConfig = this.loadConfig<EnemyConfig> (ConfigAssetsUrl.enemyConfig);
		roleConfig = this.loadConfig<RoleConfig> (ConfigAssetsUrl.roleConfig);
	}

	public T loadConfig<T> (string configUrl) {
		TextAsset configContext = AssetsManager.instance.getAssetByUrlSync<TextAsset> (configUrl);
		string context = configContext.text;
		T configData = JsonMapper.ToObject<T> (context);
		(configData as IConfig).convertData ();
		return configData;
	}
}