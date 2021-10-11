/*
 * @Author: l hy 
 * @Date: 2021-10-09 13:27:32 
 * @Description: 房间数据
 */
using UnityEngine;
public class RoomData {

	private int roomWidth;

	private int roomHeight;

	private Vector2Int roomCenter = Vector2Int.zero;

	private Vector2Int roomInMapPos = Vector2Int.zero;

	public RoomData (int roomWidth, int roomHeight, Vector2Int roomCenter, Vector2Int roomInMapPos) {
		this.roomWidth = roomWidth;
		this.roomHeight = roomHeight;
		this.roomCenter = roomCenter;
		this.roomInMapPos = roomInMapPos;
	}
}