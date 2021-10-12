/*
 * @Author: l hy 
 * @Date: 2021-09-29 08:40:01 
 * @Description: 地图生成管理
 */
using System.Collections.Generic;
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

    private Dictionary<Vector2Int, RoomData> roomDic = new Dictionary<Vector2Int, RoomData> ();

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
        this.roomDic.Add (roomData.roomInMapPos, roomData);
    }

    private void drawPassway () {
        for (int i = 0; i < this.roomOrderList.Count - 1; i++) {
            Vector2Int curRoomVec = this.roomOrderList[i];
            Vector2Int nextRoomVec = this.roomOrderList[i + 1];
            RoomData curRoomData = this.roomDic[curRoomVec];
            RoomData nextRoomData = this.roomDic[nextRoomVec];

            // 根据房间在地图中的位置坐标，判断方向。
            int horizontal = nextRoomData.roomInMapPos.x - curRoomData.roomInMapPos.x;
            int vertical = nextRoomData.roomInMapPos.y - curRoomData.roomInMapPos.y;
            if (horizontal != 0) {
                int direct = horizontal / Mathf.Abs (horizontal);
                Vector2Int curRoomCenter = curRoomData.roomCenter;
                Vector2Int rightStart = new Vector2Int (curRoomCenter.x + direct * curRoomData.roomWidth / 2, curRoomCenter.y + this.passwayHeight / 2);

                Vector2Int nextRoomCenter = nextRoomData.roomCenter;
                Vector2Int rightEnd = new Vector2Int (nextRoomCenter.x - direct * nextRoomData.roomWidth / 2, nextRoomCenter.y + this.passwayHeight / 2);

                int distance = Mathf.Abs (rightEnd.x - rightStart.x) + 1;
                for (int k = 0; k < this.passwayHeight; k++) {
                    for (int j = 0; j < distance; j++) {
                        this.tilemap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), floor);
                        if (k == 0 || k == this.passwayHeight - 1) {
                            this.tilemap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), wall);
                        }
                    }
                }
            }

            if (vertical != 0) {
                int direct = vertical / Mathf.Abs (vertical);
                Vector2Int curRoomCenter = curRoomData.roomCenter;
                Vector2Int upStart = new Vector2Int (curRoomCenter.x + this.passwayHeight / 2, curRoomCenter.y + direct * curRoomData.roomHeight / 2);

                Vector2Int nextRoomCenter = nextRoomData.roomCenter;
                Vector2Int downEnd = new Vector2Int (nextRoomCenter.x + this.passwayHeight / 2, nextRoomCenter.y - direct * nextRoomData.roomHeight / 2);

                int distance = Mathf.Abs (downEnd.y - upStart.y) + 1;
                for (int k = 0; k < distance; k++) {
                    for (int j = 0; j < this.passwayHeight; j++) {
                        this.tilemap.SetTile (new Vector3Int (upStart.x - direct * j, upStart.y + k, 0), floor);
                        if (j == 0 || j == this.passwayHeight - 1) {
                            this.tilemap.SetTile (new Vector3Int (upStart.x - direct * j, upStart.y + k, 0), wall);
                        }
                    }
                }

            }
        }
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

    /// <summary>
    ///记录地图房间生成顺序的列表 
    /// </summary>
    /// <typeparam name="Vector2Int"></typeparam>
    /// <returns></returns>
    private List<Vector2Int> roomOrderList = new List<Vector2Int> ();

    //生成一个地图 （用二维 int 数组表示）
    private int[, ] getRoomMap (int mapRow, int mapColumn, int roomCount) {
        //FIXME: 初始位置可以进行随机
        Vector2Int nowPoint = Vector2Int.zero;
        int curRoomCount = 1;

        int[, ] map = new int[mapRow, mapColumn];
        map[nowPoint.x, nowPoint.y] = 1;

        roomOrderList.Add (nowPoint);

        while (curRoomCount < roomCount) {
            nowPoint = getNextPoint (nowPoint, mapRow, mapColumn);
            if (map[nowPoint.x, nowPoint.y] == 1) {
                continue;
            }

            map[nowPoint.x, nowPoint.y] = 1;
            curRoomCount++;
            roomOrderList.Add (nowPoint);
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
        this.roomDic.Clear ();
        this.tilemap.ClearAllTiles ();
        this.roomOrderList.Clear ();
    }
}