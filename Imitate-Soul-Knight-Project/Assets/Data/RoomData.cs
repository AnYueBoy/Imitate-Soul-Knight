/*
 * @Author: l hy 
 * @Date: 2021-10-09 13:27:32 
 * @Description: 房间数据
 */
using UnityEngine;
public class RoomData {

	public int roomWidth;

	public int roomHeight;

	public Vector2Int roomCenter = Vector2Int.zero;

	public Vector2Int roomInMapPos = Vector2Int.zero;

	public Vector2Int preRoomPoint = Vector2Int.zero;

	public RoomData (Vector2Int roomInMapPos, Vector2Int preRoomPoint) {
		this.roomInMapPos = roomInMapPos;
		this.preRoomPoint = preRoomPoint;
	}
}