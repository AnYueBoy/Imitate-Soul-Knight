using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-10-19 17:22:16 
 * @Description: 玩家管理
 */
using Cinemachine;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;
public class PlayerManager : MonoBehaviour {

	private RoleControl roleControl;

	public BattleRoleData battleRoleData;

	private BaseWeapon curWeapon;

	[SerializeField]
	private CinemachineVirtualCamera cinemaCamera;

	private HashSet<PassiveTriggerItem> interfaceSet = new HashSet<PassiveTriggerItem> ();

	public void init () {
		// 构建战斗角色
		this.buildBattleRole ();
	}

	private void buildBattleRole () {
		// 构建角色实例
		GameObject playerPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (RoleAssetsUrl.player);
		GameObject playerNode = ObjectPool.instance.requestInstance (playerPrefab);
		playerNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
		this.roleControl = playerNode.GetComponent<RoleControl> ();
		cinemaCamera.Follow = this.roleControl.transform;

		// 构建角色数据
		int curRoleId = ModuleManager.instance.playerDataManager.getCurRoleId ();
		this.battleRoleData = new BattleRoleData (curRoleId);

		// 根据当前武器生成武器实例
		int curWeaponId = ModuleManager.instance.playerDataManager.getCurWeaponId ();
		string weaponUrl = ItemManager.getItemPreUrl (ItemIdEnum.NORMAL_WEAPON);
		GameObject weaponPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (weaponUrl);
		GameObject weaponNode = ObjectPool.instance.requestInstance (weaponPrefab);
		weaponNode.transform.SetParent (this.roleControl.weaponParent);
		weaponNode.transform.localPosition = Vector3.zero;
		this.curWeapon = weaponNode.GetComponent<BaseWeapon> ();
		this.triggerSwitchWeapon ();
	}

	public void localUpdate (float dt) {
		this.roleControl?.localUpdate (dt);
		this.curWeapon?.localUpdate (dt);
	}

	public Transform getPlayerTrans () {
		return this.roleControl.transform;
	}

	#region  数据访问
	public void injured (float damage) {
		float originArmor = this.battleRoleData.curArmor;
		this.battleRoleData.curArmor -= damage;
		this.battleRoleData.curArmor = Mathf.Max (0, this.battleRoleData.curArmor);

		damage -= originArmor;
		damage = Mathf.Max (0, damage);

		this.battleRoleData.curHp -= damage;
		this.battleRoleData.curHp = Mathf.Max (0, this.battleRoleData.curHp);
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);

		// TODO: 触发死亡检测
	}

	public float getCurMp () {
		return this.battleRoleData.curMp;
	}

	public void consumeMp (float value) {
		this.battleRoleData.curMp -= value;
		this.battleRoleData.curMp = Mathf.Max (0, this.battleRoleData.curMp);
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);
	}

	public void addMp (float value) {
		this.battleRoleData.curMp += value;
		this.battleRoleData.curMp = Mathf.Min (this.battleRoleData.curMaxMp, this.battleRoleData.curMp);
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);
	}

	public void addCoin (int value) {
		this.battleRoleData.curCoin += value;
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);
	}

	public void consumeCoin (int value) {
		this.battleRoleData.curCoin -= value;
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);
	}

	public void addInterfaceItem (PassiveTriggerItem item) {
		if (this.interfaceSet.Count <= 0) {
			ModuleManager.instance.inputManager.showInterfaceBtn ();
		}
		this.interfaceSet.Add (item);
	}

	public void removeInterfaceItem (PassiveTriggerItem item) {
		if (this.interfaceSet.Contains (item)) {
			this.interfaceSet.Remove (item);
			if (this.interfaceSet.Count <= 0) {
				ModuleManager.instance.inputManager.showAttackBtn ();
			}
		}
	}

	#endregion

	#region  输入调用

	public void triggerAttack () {
		this.curWeapon.launchBullet (this.roleControl.transform.localScale.x);
	}

	public void triggerInterface () {

	}

	public void triggerSwitchWeapon () {
		// FIXME: 临时逻辑
		this.curWeapon.init (TagGroup.playerBullet);
	}

	public void triggerSkill () { }
	#endregion

}