/*
 * @Author: l hy 
 * @Date: 2021-10-15 09:20:55 
 * @Description: 房间
 */
using System.Collections.Generic;
using UFramework;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room {

	public RoomData roomData;
	private TileBase doorOpenTile;
	private TileBase doorCloseTile;

	private Vector3 leftTilePoint;
	private Vector3 rightTilePoint;
	private Vector3 upTilePoint;
	private Vector3 downTilePoint;

	private PathFinding pathFinding;

	private bool curDoorClose = true;

	private bool roomActive = false;

	public Room (RoomData roomData) {
		this.roomData = roomData;
		doorOpenTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorOpenTile);
		doorCloseTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorCloseTile);
		this.roomActive = false;
		this.pathFinding = new PathFinding ();

		// 此时房间的数据并未完全赋值
	}

	public void init () {
		this.leftTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 1, this.roomData.roomCenter.y, 0));
		this.rightTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 1, this.roomData.roomCenter.y, 0));
		this.upTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 1, 0));
		this.downTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 1, 0));
		this.roomEnemyList.Clear ();

		this.createRoomItem ();

		// 初始化房间的寻路信息
		this.pathFinding.init (this.roomData.roomWidth, this.roomData.roomHeight, this.roomData.roomCenter);
	}

	public void localUpdate (float dt) {
		this.checkRoomDoor (dt);
		this.checkChest ();
	}

	private bool isEnemyClear () {
		// TODO: 性能点
		for (int i = 0; i < this.roomEnemyList.Count; i++) {
			BaseEnemy enemy = this.roomEnemyList[i];
			if (!enemy.isDead ()) {
				return false;
			}
		}

		return true;
	}

	private void checkRoomDoor (float dt) {
		if (!this.isInRoom () && !this.roomActive) {
			if (this.curDoorClose) {
				this.openDoor ();
			}
			return;
		}

		this.roomActive = true;

		// 当前怪物全部清理完成,则打开房门
		if (this.isEnemyClear ()) {
			if (this.curDoorClose) {
				this.openDoor ();
			}
			return;
		}
		if (!this.curDoorClose) {
			this.closeDoor ();
		}
	}

	private bool isAlreadySpawnChest = false;
	private void checkChest () {
		if (this.isAlreadySpawnChest) {
			return;
		}

		if (this.roomData.roomType != RoomTypeEnum.BATTLE) {
			return;
		}

		if (!this.isEnemyClear ()) {
			return;
		}

		this.isAlreadySpawnChest = true;

		this.spawnChestRandom ();

	}

	private void openDoor () {
		for (int i = 0; i < this.roomData.doorList.Count; i++) {
			Vector3Int tilePoint = this.roomData.doorList[i];
			ModuleManager.instance.mapManager.wallTileMap.SetTile (tilePoint, null);
			ModuleManager.instance.mapManager.floorTilemap.SetTile (tilePoint, doorCloseTile);
		}

		this.curDoorClose = false;
	}

	private void closeDoor () {
		for (int i = 0; i < this.roomData.doorList.Count; i++) {
			Vector3Int tilePoint = this.roomData.doorList[i];
			ModuleManager.instance.mapManager.wallTileMap.SetTile (tilePoint, doorOpenTile);
			ModuleManager.instance.mapManager.floorTilemap.SetTile (tilePoint, null);
		}
		this.curDoorClose = true;
	}

	private bool isInRoom () {
		Transform roleTrans = ModuleManager.instance.playerManager.getPlayerTrans ();
		if (roleTrans.position.x > this.leftTilePoint.x &&
			roleTrans.position.x < this.rightTilePoint.x &&
			roleTrans.position.y < this.upTilePoint.y &&
			roleTrans.position.y > this.downTilePoint.y) {
			return true;
		}
		return false;
	}

	public bool isRoomActive () {
		return this.roomActive;
	}

	private void createRoomItem () {
		RoomTypeEnum roomType = this.roomData.roomType;
		Vector3 roomCenterToWorldPos = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y, 0));

		switch (roomType) {
			case RoomTypeEnum.BATTLE:
				this.spawnEnemy ();
				break;

			case RoomTypeEnum.CHEST:
				// TODO: 生成宝箱
				break;

			case RoomTypeEnum.BLESS:
				// TODO: 生成祝福雕像
				break;

			case RoomTypeEnum.PORTAL:
				//  生成传送门
				ModuleManager.instance.itemManager.spawnItem (roomCenterToWorldPos, ItemIdEnum.PORTAL);
				break;

			case RoomTypeEnum.BORN:
				// 设置玩家出现在出生房间
				ModuleManager.instance.playerManager.getPlayerTrans ().position = roomCenterToWorldPos;
				break;
			default:
				break;
		}

	}

	public List<BaseEnemy> roomEnemyList = new List<BaseEnemy> ();

	private void spawnEnemy () {
		List<Vector3> randomPosList = this.getRandomPos (ConstValue.enemySpawnCount);
		for (int i = 0; i < ConstValue.enemySpawnCount; i++) {
			// 获取随机敌人id
			int enemyId = CommonUtil.getRandomElement<int> (this.roomData.enemyList);
			Vector3 enemyPos = randomPosList[i];
			BaseEnemy enemy = ModuleManager.instance.enemyManager.spawnEnemyById (enemyId, enemyPos, () => {
				return this.isRoomActive ();
			});

			// 设置敌人的寻路组件
			enemy.setPathFinding (this.pathFinding);

			this.roomEnemyList.Add (enemy);
		}
	}

	private List<Vector3> getRandomPos (int randomCount) {
		// TODO: 性能点
		// FIXME: 更好，更快的随机算法
		List<Vector2Int> sampleList = new List<Vector2Int> ();
		int horizontalStart = this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 3;
		int horizontalEnd = this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 3;
		int verticalStart = this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 3;
		int verticalEnd = this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 3;
		for (int i = horizontalStart; i < horizontalEnd; i++) {
			for (int j = verticalStart; j < verticalEnd; j++) {
				sampleList.Add (new Vector2Int (i, j));
			}
		}

		List<Vector3> resultList = new List<Vector3> ();
		Vector2Int randomPos = Vector2Int.zero;
		sampleList = CommonUtil.getRandomElementList<Vector2Int> (sampleList, randomCount);
		for (int i = 0; i < sampleList.Count; i++) {
			randomPos = sampleList[i];
			Vector3 resultRandomPos = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (randomPos.x, randomPos.y, 0));
			resultList.Add (resultRandomPos);
		}

		return resultList;
	}

	private void spawnChestRandom () {
		List<Vector3> randomPosList = this.getRandomPos (1);
		Vector3 randomPos = randomPosList[0];

		ModuleManager.instance.itemManager.spawnItem (randomPos, ItemIdEnum.WHITE_CHEST);
	}
}