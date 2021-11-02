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

	[SerializeField]
	private CinemachineVirtualCamera cinemaCamera;

	public void init () {
		GameObject playerPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (RoleAssetsUrl.player);
		GameObject playerNode = ObjectPool.instance.requestInstance (playerPrefab);
		playerNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
		this.roleControl = playerNode.GetComponent<RoleControl> ();
		cinemaCamera.Follow = this.roleControl.transform;
	}

	public void localUpdate (float dt) {
		this.roleControl?.localUpdate (dt);
	}

	public Transform getPlayerTrans () {
		return this.roleControl.transform;
	}
}