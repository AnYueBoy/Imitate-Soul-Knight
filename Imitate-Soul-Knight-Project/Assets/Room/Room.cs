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

	public Room (RoomData roomData) {
		this.roomData = roomData;
		doorOpenTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorOpenTile);
		doorCloseTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorCloseTile);
	}

	public void localUpdate (float dt) {
		this.checkRoomDoor ();
	}

	private void checkRoomDoor () {

	}

	private void openDoor () {
		for (int i = 0; i < this.roomData.doorList.Count; i++) {
			Vector3Int tilePoint = this.roomData.doorList[i];
			ModuleManager.instance.mapManager.wallTileMap.SetTile (tilePoint, null);
			ModuleManager.instance.mapManager.floorTilemap.SetTile (tilePoint, doorCloseTile);
		}
	}

	private void closeDoor () {
		for (int i = 0; i < this.roomData.doorList.Count; i++) {
			Vector3Int tilePoint = this.roomData.doorList[i];
			ModuleManager.instance.mapManager.wallTileMap.SetTile (tilePoint, doorOpenTile);
			ModuleManager.instance.mapManager.floorTilemap.SetTile (tilePoint, null);
		}
	}

	private bool isInRoom () {
		int leftPointX = this.roomData.roomCenter.x - this.roomData.roomWidth / 2;
		int rightPointX = this.roomData.roomCenter.x + this.roomData.roomWidth / 2;
		int upPointY = this.roomData.roomCenter.y + this.roomData.roomHeight / 2;
		int downPointX = this.roomData.roomCenter.y - this.roomData.roomHeight / 2;
		
	}
}