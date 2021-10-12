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

    #region  序列化字段

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

    [SerializeField]
    private int roomHeight;

    [SerializeField]
    private int roomWidth;

    [SerializeField]
    private int roomDistance;

    #endregion

    private Dictionary<Vector2Int, RoomData> roomDic = new Dictionary<Vector2Int, RoomData> ();

    /// <summary>
    ///记录地图房间生成顺序的列表 
    /// </summary>
    /// <typeparam name="Vector2Int"></typeparam>
    private List<Vector2Int> roomOrderList = new List<Vector2Int> ();

    private Vector2Int startRoomIndex = Vector2Int.zero;

    private Vector2Int endRoomIndex = Vector2Int.zero;

    public void generateMapClick () {
        this.resetMap ();
        this.generateMapRoom ();
        this.drawPassway ();
    }

    private void generateMapRoom () {
        int spawnRoomCount = Mathf.Min (mapRowCount * mapColumnCount, totalRoomCount);
        this.getRoomMap (mapRowCount, mapColumnCount, spawnRoomCount);
        for (int i = 0; i < this.roomOrderList.Count; i++) {
            Vector2Int roomPoint = this.roomOrderList[i];
            // 生成房间
            this.drawRoom (roomPoint.x, roomPoint.y);
        }
    }

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

        Vector2Int roomCenter = new Vector2Int (startX + roomWidth / 2, startY + roomHeight / 2);
        RoomData roomData = new RoomData (roomWidth, roomHeight, roomCenter, new Vector2Int (x, y));
        this.roomDic.Add (roomData.roomInMapPos, roomData);
    }

    private void drawPassway () {
        for (int i = 0; i < this.roomOrderList.Count - 1; i++) {
            Vector2Int nextRoomVec = this.roomOrderList[i + 1];
            RoomData nextRoomData = this.roomDic[nextRoomVec];

            // 根据房间在地图中的位置坐标，判断方向。
            int horizontal = 0;
            int vertical = 0;
            int adjustValue = 0;
            Vector2Int curRoomVec = Vector2Int.zero;
            RoomData curRoomData = null;
            do {
                curRoomVec = this.roomOrderList[i - adjustValue];
                curRoomData = this.roomDic[curRoomVec];

                horizontal = nextRoomData.roomInMapPos.x - curRoomData.roomInMapPos.x;
                vertical = nextRoomData.roomInMapPos.y - curRoomData.roomInMapPos.y;
                adjustValue++;
            } while ((horizontal != 0 && vertical != 0) || (Mathf.Abs (horizontal) > 1 || Mathf.Abs (vertical) > 1));

            // FIXME: 通过回退逻辑，会将四方联通的房间类型去掉，等待优化

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
                        this.tilemap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), floor);
                        if (j == 0 || j == this.passwayHeight - 1) {
                            this.tilemap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), wall);
                        }
                    }
                }
            }
        }
    }

    //生成一个地图 （用二维 int 数组表示）
    private void getRoomMap (int mapRow, int mapColumn, int roomCount) {
        // 随机初始位置
        Vector2Int nowPoint = new Vector2Int (CommonUtil.getRandomValue (0, mapRow - 1), CommonUtil.getRandomValue (0, mapColumn - 1));
        this.startRoomIndex = nowPoint;

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

        this.startRoomIndex = this.endRoomIndex = Vector2Int.zero;
    }
}