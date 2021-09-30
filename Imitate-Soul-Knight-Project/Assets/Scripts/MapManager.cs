/*
 * @Author: l hy 
 * @Date: 2021-09-29 08:40:01 
 * @Description: 地图生成管理
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.FrameUtil;
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
    private int[, ] RandomRoom (int minW, int minH, int maxW, int maxH) {
        int randomWidth = CommonUtil.getOddNumber (minW, maxW);
        int randomHeight = CommonUtil.getOddNumber (minH, maxH);

        int[, ] room = new int[randomWidth, randomHeight];
        for (int i = 0; i < randomWidth; i++) {
            for (int j = 0; j < randomHeight; j++) {
                room[i, j] = 1;
            }
        }
        return room;
    }

    //生成一个地图 （用二维 int 数组表示）
    private int[, ] GetRoomMap (int mapRow, int mapColumn, int roomCount) {
        Vector2Int nowPoint = Vector2Int.zero;
        int curRoomCount = 1;

        int[, ] map = new int[mapRow, mapColumn];
        map[nowPoint.x, nowPoint.y] = 1;

        while (curRoomCount < roomCount) {
            nowPoint = GetNextPoint (nowPoint, mapRow, mapColumn);
            if (map[nowPoint.x, nowPoint.y] == 1) {
                continue;
            }

            map[nowPoint.x, nowPoint.y] = 1;
            curRoomCount++;
        }

        return map;
    }
    //获取下一个房间的位置
    private Vector2Int GetNextPoint (Vector2Int nowPoint, int maxW, int maxH) {
        while (true) {
            Vector2Int endPoint = nowPoint;
            int randomValue = Random.Range (0, 4);
            switch (randomValue) {
                case 0:
                    endPoint.x += 1;
                    break;
                case 1:
                    endPoint.y += 1;
                    break;
                case 2:
                    endPoint.x -= 1;
                    break;

                case 3:
                    endPoint.y -= 1;
                    break;

                default:
                    Debug.LogWarning ("exceed value: " + randomValue);
                    break;
            }

            if (endPoint.x >= 0 && endPoint.y >= 0 && endPoint.x < maxW && endPoint.y < maxH) {
                return endPoint;
            }
        }
    }
}