/*
 * @Author: l hy 
 * @Date: 2021-10-15 09:20:55 
 * @Description: 房间
 */
using System.Collections.Generic;
using UFramework;
using UFramework.Core;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UFramework.Tween;
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
		doorOpenTile = App.Make<IAssetsManager>().GetAssetByUrlSync<TileBase> (MapAssetsUrl.doorOpenTile);
		doorCloseTile = App.Make<IAssetsManager>().GetAssetByUrlSync<TileBase> (MapAssetsUrl.doorCloseTile);
		this.roomActive = false;
		this.pathFinding = new PathFinding ();

		// 此时房间的数据并未完全赋值
	}

	public void init () {
		this.leftTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x - this.roomData.roomWidth / 2, this.roomData.roomCenter.y, 0));
		this.rightTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x + this.roomData.roomWidth / 2, this.roomData.roomCenter.y, 0));
		this.upTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y + this.roomData.roomHeight / 2, 0));
		this.downTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y - this.roomData.roomHeight / 2, 0));
		this.roomEnemyList.Clear ();

		// 初始化房间的寻路信息
		this.pathFinding.init (this.roomData.roomWidth, this.roomData.roomHeight, this.roomData.roomCenter);

		this.createRoomItem ();
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
		float leftBound = ModuleManager.instance.playerManager.leftBound;
		float rightBound = ModuleManager.instance.playerManager.rightBound;
		float topBound = ModuleManager.instance.playerManager.topBound;
		float bottomBound = ModuleManager.instance.playerManager.bottomBound;
		if (leftBound > this.leftTilePoint.x &&
			rightBound < this.rightTilePoint.x &&
			topBound < this.upTilePoint.y &&
			bottomBound > this.downTilePoint.y) {
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
		Vector3 cellSize = ModuleManager.instance.mapManager.floorTilemap.cellSize;
		roomCenterToWorldPos.x += cellSize.x / 2;
		roomCenterToWorldPos.y += cellSize.y / 2;
		switch (roomType) {
			case RoomTypeEnum.BATTLE:
				this.spawnBoxBlock ();
				this.spawnEnemy ();
				break;

			case RoomTypeEnum.CHEST:
				//  生成宝箱
				ModuleManager.instance.itemManager.spawnItem (roomCenterToWorldPos, ItemIdEnum.WEAPON_CHEST);
				break;

			case RoomTypeEnum.BLESS:
				// 生成祝福雕像
				int randomValue = CommonUtil.getRandomValue ((int) ItemIdEnum.MAGIC_STATUE, (int) ItemIdEnum.WAREWOLF_STATUE);
				Statue statue = (Statue) ModuleManager.instance.itemManager.spawnItem (roomCenterToWorldPos, (ItemIdEnum) randomValue);
				statue.init ((ItemIdEnum) randomValue);
				break;

			case RoomTypeEnum.PORTAL:
				//  生成传送门
				ModuleManager.instance.itemManager.spawnItem (roomCenterToWorldPos, ItemIdEnum.PORTAL);
				break;

			case RoomTypeEnum.BORN:
				// 设置玩家出现在出生房间
				ModuleManager.instance.playerManager.playerTrans.position = roomCenterToWorldPos;
				// FIXME: 测试逻辑
				this.spawnAllWeapon ();
				break;
			default:
				break;
		}

	}

	public List<BaseEnemy> roomEnemyList = new List<BaseEnemy> ();

	private void spawnEnemy () {
		int minX = this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 3;
		int maxX = this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 3;
		int minY = this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 3;
		int maxY = this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 3;

		List<Vector3> randomPosList = this.getRandomCellWorldPos (ConstValue.enemySpawnCount, minX, maxX, minY, maxY);
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

	private void spawnBoxBlock () {
		int minX = this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 3;
		int maxX = this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 3;
		int minY = this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 3;
		int maxY = this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 3;
		GameObject boxBlockPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (MapAssetsUrl.boxBlockItem);
		List<Vector3> randomPosList = this.getRandomCellWorldPos (ConstValue.battleRoomBlockCount, minX, maxX, minY, maxY);
		for (int i = 0; i < randomPosList.Count; i++) {
			Vector3 pos = randomPosList[i];
			GameObject boxBlockNode = ObjectPool.instance.requestInstance (boxBlockPrefab);
			boxBlockNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
			boxBlockNode.transform.position = pos;

			Cell cell = this.pathFinding.getGridByPos (pos);
			cell.isObstacle = true;

			boxBlockNode.GetComponent<DestructibleBlock> ().init ((Vector3 blockPos) => {
				Cell targetCell = this.pathFinding.getGridByPos (pos);
				targetCell.isObstacle = false;
			});
		}
	}

	/// <summary>
	/// 获取非障碍的随机位置
	/// </summary>
	private List<Vector3> getRandomCellWorldPos (int randomCount, int minX, int maxX, int minY, int maxY) {
		// 获取的是cell的本地坐标
		List<Vector3Int> cellLocalList = new List<Vector3Int> ();

		int curPointCount = 0;
		while (curPointCount < randomCount) {
			int randomX = CommonUtil.getRandomValue (minX, maxX - 1);
			int randomY = CommonUtil.getRandomValue (minY, maxY - 1);
			Vector3Int randomPos = new Vector3Int (randomX, randomY, 0);
			bool isSatisfyCondition = true;
			for (int i = 0; i < cellLocalList.Count; i++) {
				Vector3Int resultRandomPos = cellLocalList[i];
				Cell cell = this.pathFinding.getGridByCellPos (resultRandomPos);
				if (cell.isObstacle) {
					isSatisfyCondition = false;
					break;
				}
				if (resultRandomPos.x == randomPos.x && resultRandomPos.y == randomPos.y) {
					isSatisfyCondition = false;
					break;
				}
			}

			if (isSatisfyCondition) {
				curPointCount++;
				cellLocalList.Add (randomPos);
			}
		}

		List<Vector3> cellWorldList = new List<Vector3> ();
		for (int i = 0; i < cellLocalList.Count; i++) {
			Vector3Int resultPos = cellLocalList[i];
			Vector3 resultRandomPos = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (resultPos.x, resultPos.y, 0));
			Vector3 cellSize = ModuleManager.instance.mapManager.floorTilemap.cellSize;
			resultRandomPos.x += cellSize.x / 2;
			resultRandomPos.y += cellSize.y / 2;
			cellWorldList.Add (resultRandomPos);
		}

		return cellWorldList;
	}

	private void spawnChestRandom () {
		int minX = this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 3;
		int maxX = this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 3;
		int minY = this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 3;
		int maxY = this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 3;
		List<Vector3> randomPosList = this.getRandomCellWorldPos (1, minX, maxX, minY, maxY);
		Vector3 randomPos = randomPosList[0];

		ModuleManager.instance.itemManager.spawnItem (randomPos, ItemIdEnum.WHITE_CHEST);
	}

	private void spawnAllWeapon () {
		List<WeaponConfigData> allWeaponList = ModuleManager.instance.configManager.weaponConfig.getAllWeaponConfigData ();
		int minX = this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 3;
		int maxX = this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 3;
		int minY = this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 3;
		int maxY = this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 3;
		int weaponCount = allWeaponList.Count;
		List<Vector3> randomPosList = this.getRandomCellWorldPos (weaponCount, minX, maxX, minY, maxY);

		for (int i = 0; i < allWeaponList.Count; i++) {
			WeaponConfigData weaponConfigData = allWeaponList[i];
			Vector3 randomPos = randomPosList[i];
			ModuleManager.instance.itemManager.spawnWeapon (randomPos, (ItemIdEnum) weaponConfigData.id);
		}

	}
}