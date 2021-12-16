/*
 * @Author: l hy 
 * @Date: 2021-11-03 17:18:04 
 * @Description: 寻路
 */
using System.Collections.Generic;
using UFramework;
using UFramework.FrameUtil;
using UnityEngine;
public class PathFinding {

	private List<Cell> openList = new List<Cell> ();

	private List<Cell> closeList = new List<Cell> ();

	private Cell[, ] cellInfoArray;

	private int roomWidth;

	private int roomHeight;

	private Vector2Int roomCenter;

	public void init (int roomWidth, int roomHeight, Vector2Int roomCenter) {
		this.roomWidth = roomWidth;
		this.roomHeight = roomHeight;
		this.roomCenter = roomCenter;

		// 扫描房间信息
		this.scanningRoomInfo ();
	}

	private void scanningRoomInfo () {
		cellInfoArray = new Cell[roomWidth, roomHeight];
		int horizontalStart = roomCenter.x - roomWidth / 2;
		int horizontalEnd = roomCenter.x + roomWidth / 2;
		int verticalStart = roomCenter.y + roomHeight / 2;
		int verticalEnd = roomCenter.y - roomHeight / 2;

		// 列扫描
		for (int i = horizontalStart, k = 0; i <= horizontalEnd; i++, k++) {
			for (int j = verticalStart, m = 0; j >= verticalEnd; j--, m++) {
				Vector3Int tilePos = new Vector3Int (i, j, 0);
				bool isObstacle = true;

				if (ModuleManager.instance.mapManager.wallTileMap.HasTile (tilePos)) {
					isObstacle = true;
				} else {
					isObstacle = false;
				}

				Vector3 worldPos = ModuleManager.instance.mapManager.cellToWorldPos (tilePos);
				Cell cellInfo = new Cell (isObstacle, worldPos, k, m);
				cellInfoArray[k, m] = cellInfo;
			}
		}
	}

	public void updateCellInfo (Vector3Int cellPos, bool isObstacle) {
		Cell cellInfo = this.getGridByCellPos (cellPos);
		cellInfo.isObstacle = isObstacle;
	}

	public Vector3 getRandomCellPos () {
		int x = CommonUtil.getRandomValue (2, this.roomWidth - 2);
		int y = CommonUtil.getRandomValue (2, this.roomHeight - 2);

		return this.cellInfoArray[x, y].worldPos;
	}

	public List<Vector3> findPath (Vector3 origin, Vector3 target) {
		this.openList.Clear ();
		this.closeList.Clear ();

		Cell startCell = this.getGridByPos (origin);
		Cell endCell = this.getGridByPos (target);

		if (startCell == null || endCell == null) {
			return null;
		}

		// 首位元素不用，以便于二叉堆的排序
		this.openList.Add (new Cell (false, Vector3.zero, -1, -1));

		this.openList.Add (startCell);

		while (this.openList.Count > 1) {
			Cell curCell = this.openList[1];

			this.binaryHeapRemove ();

			this.closeList.Add (curCell);

			if (curCell == endCell) {
				return generatePath (startCell, endCell);
			}

			List<Cell> aroundCellList = this.getAroundCellFour (curCell);
			for (int i = 0; i < aroundCellList.Count; i++) {
				Cell cell = aroundCellList[i];
				if (cell == null) {
					continue;
				}

				if (cell.isObstacle || this.closeList.IndexOf (cell) != -1) {
					continue;
				}

				float newCost = curCell.gCost + this.getManhattan (curCell, cell);
				if (newCost < cell.gCost || this.openList.IndexOf (cell) == -1) {
					cell.gCost = newCost;

					cell.hCost = this.getManhattan (cell, endCell);

					cell.parent = curCell;
					if (this.openList.IndexOf (cell) == -1) {
						this.binaryHeapAdd (cell);
					} else {
						this.binaryHeapReorder (cell);
					}
				}
			}
		}

		Debug.Log ("未找到路径");
		return null;
	}

	private float getManhattan (Cell firstCell, Cell secondCell) {
		int centX = Mathf.Abs (firstCell.x - secondCell.x);
		int centY = Mathf.Abs (firstCell.y - secondCell.y);
		return ConstValue.horAndVerCost * (centX + centY);
	}

	private List<Vector3> generatePath (Cell startCell, Cell endCell) {
		if (endCell == null) {
			return null;
		}

		List<Cell> path = new List<Cell> ();

		while (endCell != startCell) {
			path.Add (endCell);
			endCell = endCell.parent;
		}

		// 翻转路径
		path.Reverse ();

		List<Vector3> pathInfo = new List<Vector3> ();
		for (int i = 0; i < path.Count; i++) {
			Cell cell = path[i];
			if (cell == null) {
				continue;
			}
			pathInfo.Add (cell.worldPos);
		}

		return pathInfo;

	}

	public Cell getGridByPos (Vector3 targetPos) {
		Vector3Int cellPos = ModuleManager.instance.mapManager.floorTilemap.WorldToCell (targetPos);
		return this.getGridByCellPos (cellPos);
	}

	public Cell getGridByCellPos (Vector3Int cellPos) {
		// 映射到数组
		int x = cellPos.x + this.roomWidth / 2 - this.roomCenter.x;
		int y = -cellPos.y + this.roomHeight / 2 + this.roomCenter.y;

		if (x < 0 || x >= this.roomWidth || y < 0 || y >= this.roomHeight) {
			return null;
		}

		return this.cellInfoArray[x, y];
	}

	private List<Cell> getAroundCellEight (Cell targetCell) {
		List<Cell> cellList = new List<Cell> ();
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (i == 0 && j == 0) {
					continue;
				}

				int indexX = targetCell.x + i;
				int indexY = targetCell.y + j;
				if (indexX >= 0 && indexX < this.roomWidth && indexY >= 0 && indexY < this.roomHeight) {
					cellList.Add (this.cellInfoArray[indexX, indexY]);
				}
			}
		}
		return cellList;
	}

	private List<Cell> getAroundCellFour (Cell targetCell) {
		List<Cell> cellList = new List<Cell> ();
		Vector2Int upPoint = new Vector2Int (targetCell.x, targetCell.y + 1);
		Vector2Int downPoint = new Vector2Int (targetCell.x, targetCell.y - 1);

		Vector2Int leftPoint = new Vector2Int (targetCell.x - 1, targetCell.y);
		Vector2Int rightPoint = new Vector2Int (targetCell.x + 1, targetCell.y);

		if (upPoint.x >= 0 && upPoint.x < this.roomWidth && upPoint.y >= 0 && upPoint.y < this.roomHeight) {
			cellList.Add (this.cellInfoArray[upPoint.x, upPoint.y]);
		}

		if (downPoint.x >= 0 && downPoint.x < this.roomWidth && downPoint.y >= 0 && downPoint.y < this.roomHeight) {
			cellList.Add (this.cellInfoArray[downPoint.x, downPoint.y]);
		}

		if (leftPoint.x >= 0 && leftPoint.x < this.roomWidth && leftPoint.y >= 0 && leftPoint.y < this.roomHeight) {
			cellList.Add (this.cellInfoArray[leftPoint.x, leftPoint.y]);
		}

		if (rightPoint.x >= 0 && rightPoint.x < this.roomWidth && rightPoint.y >= 0 && rightPoint.y < this.roomHeight) {
			cellList.Add (this.cellInfoArray[rightPoint.x, rightPoint.y]);
		}

		return cellList;
	}

	private void binaryHeapAdd (Cell cell) {
		int lastIndex = this.openList.Count;
		this.openList.Add (cell);

		while (lastIndex > 1) {
			int halfIndex = lastIndex >> 1;
			if (this.openList[lastIndex].fCost >= this.openList[halfIndex].fCost) {
				return;
			}

			Cell tempCell = this.openList[lastIndex];
			this.openList[lastIndex] = this.openList[halfIndex];
			this.openList[halfIndex] = tempCell;

			lastIndex = halfIndex;
		}
	}

	private void binaryHeapRemove () {
		int lastIndex = this.openList.Count - 1;
		this.openList[1] = this.openList[lastIndex];

		this.openList.RemoveAt (lastIndex);

		lastIndex = this.openList.Count - 1;

		int headIndex = 1;
		while ((headIndex << 1) + 1 <= lastIndex) {
			int leftChildIndex = headIndex << 1;
			int rightChildIndex = leftChildIndex + 1;

			int minIndex = this.openList[leftChildIndex].fCost <= this.openList[rightChildIndex].fCost?leftChildIndex : rightChildIndex;

			if (this.openList[headIndex].fCost <= this.openList[minIndex].fCost) {
				return;
			}

			Cell tempCell = this.openList[headIndex];
			this.openList[headIndex] = this.openList[minIndex];
			this.openList[minIndex] = tempCell;
			headIndex = minIndex;
		}
	}

	private void binaryHeapReorder (Cell cell) {
		int lastIndex = this.openList.IndexOf (cell);
		if (lastIndex == -1) {
			Debug.LogError ("需要重排序的单元不存在");
			return;
		}

		while (lastIndex > 1) {
			int halfIndex = lastIndex >> 1;
			if (this.openList[lastIndex].fCost >= this.openList[halfIndex].fCost) {
				return;
			}

			Cell tempCell = this.openList[lastIndex];
			this.openList[lastIndex] = this.openList[halfIndex];
			this.openList[halfIndex] = tempCell;
			lastIndex = halfIndex;
		}
	}
}