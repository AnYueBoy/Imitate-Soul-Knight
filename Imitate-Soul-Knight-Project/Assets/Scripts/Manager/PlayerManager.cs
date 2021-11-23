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

	[SerializeField]
	private CinemachineVirtualCamera cinemaCamera;

	public void init () {
		GameObject playerPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (RoleAssetsUrl.player);
		GameObject playerNode = ObjectPool.instance.requestInstance (playerPrefab);
		playerNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
		this.roleControl = playerNode.GetComponent<RoleControl> ();
		cinemaCamera.Follow = this.roleControl.transform;

		this.buildBattleRoleData ();
	}

	private void buildBattleRoleData () {
		int curRoleId = ModuleManager.instance.playerDataManager.getCurRoleId ();
		this.battleRoleData = new BattleRoleData (curRoleId);
	}

	public void localUpdate (float dt) {
		this.roleControl?.localUpdate (dt);
	}

	public Transform getPlayerTrans () {
		return this.roleControl.transform;
	}

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

}