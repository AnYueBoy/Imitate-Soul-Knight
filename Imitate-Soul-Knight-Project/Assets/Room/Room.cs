/*
 * @Author: l hy 
 * @Date: 2021-10-15 09:20:55 
 * @Description: 房间
 */
using UFramework;
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

	private bool curDoorClose = true;

	private bool roomActive = false;

	private float monsterTimer = 0;

	public Room (RoomData roomData) {
		this.roomData = roomData;
		doorOpenTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorOpenTile);
		doorCloseTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorCloseTile);
		this.roomActive = false;

		// 此时房间的数据并未完全赋值
	}

	public void init () {
		this.leftTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x - this.roomData.roomWidth / 2 + 1, this.roomData.roomCenter.y, 0));
		this.rightTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x + this.roomData.roomWidth / 2 - 1, this.roomData.roomCenter.y, 0));
		this.upTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y + this.roomData.roomHeight / 2 - 1, 0));
		this.downTilePoint = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y - this.roomData.roomHeight / 2 + 1, 0));

		this.createRoomItem ();
	}

	public void localUpdate (float dt) {
		this.checkRoomDoor (dt);
	}

	private void checkRoomDoor (float dt) {
		if (!this.isInRoom () && !this.roomActive) {
			if (this.curDoorClose) {
				this.openDoor ();
			}
			return;
		}

		this.roomActive = true;

		this.monsterTimer += dt;

		// TODO: 当前怪物全部清理完成,则打开房门

		if (this.monsterTimer > 5) {
			if (this.curDoorClose) {
				this.openDoor ();
			}
			return;
		}

		if (!this.curDoorClose) {
			this.closeDoor ();
		}
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

	private void createRoomItem () {
		RoomTypeEnum roomType = this.roomData.roomType;
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
				// TODO: 生成传送门
				break;

			case RoomTypeEnum.BORN:
				// 设置玩家出现在出生房间
				Vector3 worldPos = ModuleManager.instance.mapManager.wallTileMap.CellToWorld (new Vector3Int (this.roomData.roomCenter.x, this.roomData.roomCenter.y, 0));
				ModuleManager.instance.playerManager.getPlayerTrans ().position = worldPos;
				break;
			default:
				break;
		}

	}

	private void spawnEnemy () { }
}