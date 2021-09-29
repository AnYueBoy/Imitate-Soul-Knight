/*
 * @Author: l hy 
 * @Date: 2021-09-29 08:40:01 
 * @Description: 地图生成管理s
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    private int seed = 123456;

    private int mapMaxRowCount;

    private int mapMaxColumnCount;

    private int totalRoomCount;

    private int roomMaxWidth;

    private int roomMaxHeight;

    private int roomMinWidth;

    private int roomMinHeight;

    private int roomDistance;

    private TileBase floor;

    private TileBase wall;

    private Tilemap tilemap;

    private int[, ] roomMap;

    private List<Vector2Int> _centerPoint;

    private Dictionary<Vector2Int, int> _mapPoint;

    //画出地图
    private void DrawMap () {
        this.DrawFloor ();
        this.DrawRoad ();
    }
    //画出房间
    private void DrawRoom (int roomX, int roomY) { }
    //画出路
    private void DrawRoad () { }
    //画出地板和墙壁
    private void DrawFloor () { }
    //生成房间 （用二维 int 数组表示）
    private int[, ] RandomRoom (int maxW, int maxH, int minW, int minH, out int width, out int height) {
        width = 1;
        height = 1;
        return null;
    }
    //生成一个地图 （用二维 int 数组表示）
    private int[, ] GetRoomMap (int mapW, int mapH, int roomCount) {
        return null;
    }
    //获取下一个房间的位置
    private Vector2Int GetNextPoint (Vector2Int nowPoint, int maxW, int maxH) {
        return Vector2Int.zero;
    }
}