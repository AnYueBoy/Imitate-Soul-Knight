/*
 * @Author: l hy 
 * @Date: 2021-10-19 17:22:16 
 * @Description: 玩家管理
 */
using System.Collections.Generic;
using Cinemachine;
using UFramework;
using UFramework.FrameUtil;
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

		if (Input.touchCount <= 0) {
			return;
		}

		Touch touch = Input.touches[0];
		Vector3 touchPos = touch.position;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (touchPos);
		List<Vector3> posList = this.pathFinding.findPath (this.roleControl.transform.position, worldPoint);
		this.drawPath (posList);

	}

	public Transform getPlayerTrans () {
		return this.roleControl.transform;
	}

	protected PathFinding pathFinding;

	public void setPathFinding (PathFinding pathFinding) {
		this.pathFinding = pathFinding;
	}

	private void drawPath (List<Vector3> posList) {
		if (posList == null) {
			return;
		}
		for (int i = 0; i < posList.Count - 1; i++) {
			Vector3 curPos = posList[i];
			Vector3 nextPos = posList[i + 1];

			CommonUtil.drawLine (curPos, nextPos, Color.red);
		}

		// this.roleControl.transform.position = posList[posList.Count - 1];

	}
}