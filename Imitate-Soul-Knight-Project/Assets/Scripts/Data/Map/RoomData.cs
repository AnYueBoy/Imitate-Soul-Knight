using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-10-09 13:27:32 
 * @Description: 房间数据
 */
using UnityEngine;
public class RoomData : BaseData {

	public int roomWidth;

	public int roomHeight;

	public RoomTypeEnum roomType;

	/// <summary>
	///方格左下角 
	/// </summary>
	public Vector2Int roomCenter = Vector2Int.zero;

	public Vector2Int roomInMapPos = Vector2Int.zero;

	public Vector2Int preRoomPoint = Vector2Int.zero;

	public List<Vector3Int> doorList = new List<Vector3Int> ();

	public List<int> enemyList = new List<int> ();

	public List<int> dropWeapon = new List<int> ();

	public int boos;

	public RoomData (Vector2Int roomInMapPos, Vector2Int preRoomPoint, RoomTypeEnum roomType, LevelData levelData) {
		this.roomInMapPos = roomInMapPos;
		this.preRoomPoint = preRoomPoint;
		this.roomType = roomType;

		this.enemyList = levelData.enemyList;
		this.dropWeapon = levelData.dropWeapon;
		this.boos = levelData.boss;
	}
}