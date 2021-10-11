using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-09-29 08:40:01 
 * @Description: 地图生成管理
 */
using UFramework.FrameUtil;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    [SerializeField]
    private int mapRowCount;

    [SerializeField]
    private int mapColumnCount;

    [SerializeField]
    private int totalRoomCount;

    [SerializeField]
    private TileBase floor;

    [SerializeField]
    private TileBase wall;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private int passwayHeight;

    private List<RoomData> roomList;

    public void generateMapClick () {
        this.resetMap ();
        this.generateMapRoom ();
        this.drawPassway ();
    }

    private void generateMapRoom () {
        int spawnRoomCount = Mathf.Min (mapRowCount * mapColumnCount, totalRoomCount);
        int[, ] roomMap = this.getRoomMap (mapRowCount, mapColumnCount, spawnRoomCount);
        for (int i = 0; i < mapRowCount; i++) {
            for (int j = 0; j < mapColumnCount; j++) {
                if (roomMap[i, j] == 1) {
                    // 生成房间
                    this.drawRoom (i, j);
                }
            }
        }
    }

    [SerializeField]
    private int roomHeight;

    [SerializeField]
    private int roomWidth;

    [SerializeField]
    private int roomDistance;

    private void drawRoom (int x, int y) {
        int startX = (roomWidth + roomDistance) * x;
        int startY = (roomHeight + roomDistance) * y;
        for (int i = 0; i < roomWidth; i++) {
            for (int j = 0; j < roomHeight; j++) {
                // 绘制地板
                tilemap.SetTile (new Vector3Int (startX + i, startY + j, 0), floor);
                // 绘制墙体
                if (i == 0 || i == roomWidth - 1) {
                    tilemap.SetTile (new Vector3Int (startX + i, startY + j, 0), wall);
                } else {
                    if (j == 0 || j == roomHeight - 1) {
                        tilemap.SetTile (new Vector3Int (startX + i, startY + j, 0), wall);
                    }
                }
            }
        }

        Vector2Int roomCenter = new Vector2Int (startX + Mathf.FloorToInt (roomWidth / 2), startY + Mathf.FloorToInt (roomHeight / 2));
        RoomData roomData = new RoomData (roomWidth, roomHeight, roomCenter, new Vector2Int (x, y));
        this.roomList.Add (roomData);
    }

    private void drawPassway () {

    }

    //生成房间 （用二维 int 数组表示）
    private int[, ] randomRoom (int minW, int minH, int maxW, int maxH) {
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
    private int[, ] getRoomMap (int mapRow, int mapColumn, int roomCount) {
        //FIXME: 初始位置可以进行随机
        Vector2Int nowPoint = Vector2Int.zero;
        int curRoomCount = 1;

        int[, ] map = new int[mapRow, mapColumn];
        map[nowPoint.x, nowPoint.y] = 1;

        while (curRoomCount < roomCount) {
            nowPoint = getNextPoint (nowPoint, mapRow, mapColumn);
            if (map[nowPoint.x, nowPoint.y] == 1) {
                continue;
            }

            map[nowPoint.x, nowPoint.y] = 1;
            curRoomCount++;
        }

        return map;
    }
    //获取下一个房间的位置
    private Vector2Int getNextPoint (Vector2Int nowPoint, int maxW, int maxH) {
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

    private void resetMap () {
        this.roomList.Clear ();
        this.tilemap.ClearAllTiles ();
    }
}