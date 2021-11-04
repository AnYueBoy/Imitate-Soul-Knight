using System.Collections.Generic;
using UFramework;
/*
 * @Author: l hy 
 * @Date: 2021-11-03 17:18:04 
 * @Description: 寻路
 */
using UnityEngine;
public class PathFinding {

	private List<Cell> openList = new List<Cell> ();

	private List<Cell> closeList = new List<Cell> ();

	public List<Vector3> resultList = new List<Vector3> ();

	private Cell[, ] cellInfoArray;

	private readonly float horAndVerCost = 10;

	private readonly float diagonalCost = 14;

	private int roomWidth;

	private int roomHeight;

	private Vector3Int roomCenter;

	public void init (int roomWidth, int roomHeight, Vector3Int roomCenter) {
		this.roomWidth = roomWidth;
		this.roomHeight = roomHeight;
		this.roomCenter = roomCenter;
		cellInfoArray = new Cell[roomWidth, roomHeight];
	}

	public void findPath (Vector3 origin, Vector3 target) {
		this.openList.Clear ();
		this.closeList.Clear ();
		this.resultList.Clear ();

		Cell startGrid = this.getGridByPos (origin);
		Cell endGrid = this.getGridByPos (target);

		this.openList.Add (startGrid);

		while (this.openList.Count > 0) {
			Cell curCell = this.openList[0];
		}

	}

	private Cell getGridByPos (Vector3 targetPos) {
		Vector3Int cellPos = ModuleManager.instance.mapManager.floorTilemap.WorldToCell (targetPos);

		// 映射到数组
		int x = cellPos.x + this.roomWidth / 2 - this.roomCenter.x;
		int y = -cellPos.y + this.roomHeight / 2 + this.roomCenter.y;

		return this.cellInfoArray[x, y];
	}

}