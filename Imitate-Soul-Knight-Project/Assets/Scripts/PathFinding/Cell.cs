/*
 * @Author: l hy 
 * @Date: 2021-11-03 17:10:12 
 * @Description: 格子单元
 */
using UnityEngine;

public class Cell {
    public bool isObstacle;

    public Vector3 pos;

    public int x;

    public int y;

    public float gCost;

    public float hCost;

    public float fCost {
        get {
            return this.gCost + hCost;
        }
    }

    public Cell parent;

    public Cell (
        bool isObstacle,
        Vector3 pos,
        int x,
        int y
    ) {
        this.isObstacle = isObstacle;
        this.pos = pos;
        this.x = x;
        this.y = y;
    }
}