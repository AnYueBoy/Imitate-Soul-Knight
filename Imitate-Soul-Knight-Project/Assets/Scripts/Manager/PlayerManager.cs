/*
 * @Author: l hy 
 * @Date: 2021-10-19 17:22:16 
 * @Description: 玩家管理
 */
using System.Collections.Generic;
using Cinemachine;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;
public class PlayerManager : MonoBehaviour {

	private RoleControl roleControl;

	public BattleRoleData battleRoleData;

	private BaseWeapon curWeapon;

	private BaseWeapon spareMeleeWeapon;

	[SerializeField] private CinemachineVirtualCamera cinemaCamera;

	private HashSet<PassiveTriggerItem> interfaceSet = new HashSet<PassiveTriggerItem> ();

	private List<BaseWeapon> weaponList = new List<BaseWeapon> ();

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
		BaseWeapon curWeapon = ModuleManager.instance.itemManager.spawnWeapon (Vector3.zero, (ItemIdEnum) curWeaponId);

		// TODO:生成备用近战武器

		this.equipmentWeapon (curWeapon);
	}

	public void localUpdate (float dt) {
		this.roleControl?.localUpdate (dt);
		this.curWeapon?.localUpdate (dt);
	}

	public Transform getPlayerTrans () {
		return this.roleControl.transform;
	}

	public Transform getMeleeEffectTransform () {
		return this.roleControl.getEffectTransform ();
	}

	private bool isProtected = false;
	private readonly float protectedTime = 0.64f;

	#region  数据访问
	public void injured (float damage) {
		if (this.isProtected) {
			return;
		}

		float originArmor = this.battleRoleData.curArmor;
		this.battleRoleData.curArmor -= damage;
		this.battleRoleData.curArmor = Mathf.Max (0, this.battleRoleData.curArmor);

		damage -= originArmor;
		damage = Mathf.Max (0, damage);

		this.battleRoleData.curHp -= damage;
		this.battleRoleData.curHp = Mathf.Max (0, this.battleRoleData.curHp);
		ListenerManager.instance.trigger (EventName.ATTRIBUTE_CHANGE);

		// TODO: 触发死亡检测

		this.isProtected = true;
		this.roleControl.hurtEffect (this.protectedTime, () => {
			this.isProtected = false;
		});

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

	public void equipmentWeapon (BaseWeapon weapon) {
		if (this.weaponList.Count < this.battleRoleData.weaponSlotCount) {
			if (this.weaponList.Count == 0) {
				this.weaponList.Add (weapon);
			} else {
				this.curWeapon.gameObject.SetActive (false);
				this.weaponList.Insert (0, weapon);
			}

			BaseWeapon curWeapon = this.weaponList[0];

			this.setCurWeapon (curWeapon);

			return;
		}

		this.curWeapon.transform.SetParent (ModuleManager.instance.gameObjectTrans);
		this.curWeapon.transform.position = weapon.transform.position;
		this.curWeapon.transform.eulerAngles = weapon.transform.eulerAngles;
		this.curWeapon.GetComponent<PassiveTriggerItem> ().reset ();

		this.weaponList.RemoveAt (0);
		this.weaponList.Insert (0, weapon);
		this.setCurWeapon (weapon);
	}

	private void setCurWeapon (BaseWeapon weapon) {
		this.curWeapon = weapon;
		this.curWeapon.gameObject.SetActive (true);
		this.curWeapon.equipment (LayerGroup.playerBullet);
		this.curWeapon.transform.SetParent (this.roleControl.weaponParent);
		this.curWeapon.transform.localPosition = Vector3.zero;
		this.curWeapon.transform.localEulerAngles = Vector3.zero;
		this.curWeapon.transform.localScale = Vector3.one;
		this.curWeapon.GetComponent<PassiveTriggerItem> ().triggerSelf ();

		this.setWeaponInfo (this.curWeapon.getWeaponMpConsume ());
	}

	private void setWeaponInfo (int consume) {
		Sprite weaponSprite = this.curWeapon.GetComponent<SpriteRenderer> ().sprite;
		ModuleManager.instance.inputManager.setWeaponInfo (weaponSprite, consume);
	}

	#endregion

	#region  输入调用

	public void triggerAttack () {
		// TODO: 距离过近，进行近战攻击
		this.curWeapon.attack (this.roleControl.transform.localScale.x);
	}

	public void triggerInterface () {
		PassiveTriggerItem interfaceItem = this.getCloseInterfaceItem ();
		if (interfaceItem == null) {
			Debug.LogWarning ("can not get close interface item");
			return;
		}
		interfaceItem.triggerHandler ();
	}

	public void triggerSwitchWeapon () {
		if (this.weaponList.Count <= 1) {
			return;
		}

		BaseWeapon firstElement = this.weaponList[0];
		this.weaponList.RemoveAt (0);
		this.weaponList.Add (firstElement);
		firstElement.gameObject.SetActive (false);

		this.setCurWeapon (this.weaponList[0]);
	}

	public void triggerSkill () { }
	#endregion

	private PassiveTriggerItem getCloseInterfaceItem () {
		float closeDistance = float.MaxValue;
		PassiveTriggerItem targetItem = null;
		foreach (PassiveTriggerItem item in this.interfaceSet) {
			float itemDistance = item.getSelfToPlayerDis ();
			if (itemDistance > closeDistance) {
				continue;
			}

			closeDistance = itemDistance;
			targetItem = item;
		}

		return targetItem;
	}

}