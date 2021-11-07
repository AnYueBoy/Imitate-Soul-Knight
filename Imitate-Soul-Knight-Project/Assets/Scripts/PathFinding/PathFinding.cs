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

	private Cell[, ] cellInfoArray;

	private readonly float horAndVerCost = 10;

	private readonly float diagonalCost = 14;

	private int roomWidth;

	private int roomHeight;

	private Vector2Int roomCenter;

	public void init (int roomWidth, int roomHeight, Vector2Int roomCenter) {
		this.roomWidth = roomWidth;
		this.roomHeight = roomHeight;
		this.roomCenter = roomCenter;
		cellInfoArray = new Cell[roomWidth, roomHeight];

		int horizontalStart = roomCenter.x - roomWidth / 2;
		int horizontalEnd = roomCenter.x + roomWidth / 2;
		int verticalStart = roomCenter.y + roomHeight / 2;
		int verticalEnd = roomCenter.y - roomHeight / 2;
		for (int i = verticalStart, k = 0; i <= verticalEnd; i--, k++) {
			for (int j = horizontalStart, m = 0; j <= horizontalEnd; j++, m++) {
				Vector3Int tilePos = new Vector3Int (i, j, 0);
				if (ModuleManager.instance.mapManager.floorTilemap.HasTile (tilePos)) {
					Vector3 worldPos = ModuleManager.instance.mapManager.floorTilemap.CellToWorld (tilePos);
					Cell cellInfo = new Cell (false, worldPos, k, m);
				}
			}
		}
	}

	public List<Vector3> findPath (Vector3 origin, Vector3 target) {
		this.openList.Clear ();
		this.closeList.Clear ();

		Cell startCell = this.getGridByPos (origin);
		Cell endCell = this.getGridByPos (target);

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

			List<Cell> aroundCellList = this.getAroundCell (curCell);
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

		return null;
	}

	private float getManhattan (Cell firstCell, Cell secondCell) {
		int centX = Mathf.Abs (firstCell.x - secondCell.x);
		int centY = Mathf.Abs (firstCell.y - secondCell.y);
		return this.horAndVerCost * (centX + centY);
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
			pathInfo.Add (cell.pos);
		}

		return pathInfo;

	}

	private Cell getGridByPos (Vector3 targetPos) {
		Vector3Int cellPos = ModuleManager.instance.mapManager.floorTilemap.WorldToCell (targetPos);

		// 映射到数组
		int x = cellPos.x + this.roomWidth / 2 - this.roomCenter.x;
		int y = -cellPos.y + this.roomHeight / 2 + this.roomCenter.y;

		return this.cellInfoArray[x, y];
	}

	public List<Cell> getAroundCell (Cell targetCell) {
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