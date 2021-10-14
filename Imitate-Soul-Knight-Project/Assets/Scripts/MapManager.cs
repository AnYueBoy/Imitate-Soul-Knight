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
    private Tilemap floorTilemap;

    [SerializeField]
    private Tilemap wallTileMap;

    [SerializeField]
    private int passwayHeight;

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
        this.drawRoom ();
        this.drawPassway ();
    }

    private void generateMapRoom () {
        int spawnRoomCount = Mathf.Min (mapRowCount * mapColumnCount, totalRoomCount);
        this.getRoomMap (mapRowCount, mapColumnCount, spawnRoomCount);
    }

    [SerializeField]
    private int minRoomWidth;
    [SerializeField]
    private int maxRoomWidth;
    [SerializeField]
    private int minRoomHeight;
    [SerializeField]
    private int maxRoomHeight;

    private void drawRoom () {
        //  随机房间大小
        int startX = 0;
        int startY = 0;
        Vector2Int curRoomPoint = Vector2Int.zero;
        Vector2Int preRoomPoint = Vector2Int.zero;
        Vector2Int roomCenter = Vector2Int.zero;

        for (int i = 0; i < this.roomOrderList.Count; i++) {
            curRoomPoint = this.roomOrderList[i];
            RoomData curRoomData = this.roomDic[curRoomPoint];

            int randomWidth = CommonUtil.getOddNumber (minRoomWidth, maxRoomWidth);
            int randomHeight = CommonUtil.getOddNumber (minRoomHeight, maxRoomHeight);

            int horizontal = 1;
            int vertical = 1;
            if (i > 0) {
                preRoomPoint = curRoomData.preRoomPoint;
                RoomData preRoomData = this.roomDic[preRoomPoint];
                horizontal = curRoomPoint.x - preRoomPoint.x;
                vertical = curRoomPoint.y - preRoomPoint.y;
                if ((horizontal == 0 && vertical == 0) || (horizontal != 0 && vertical != 0)) {
                    Debug.LogError ("error value.");
                }

                if (horizontal != 0) {
                    roomCenter.x = preRoomData.roomCenter.x + horizontal * maxRoomWidth + horizontal * roomDistance;
                    roomCenter.y = preRoomData.roomCenter.y;

                    startX = roomCenter.x - horizontal * randomWidth / 2;
                    startY = roomCenter.y - randomHeight / 2;
                }

                if (vertical != 0) {
                    roomCenter.x = preRoomData.roomCenter.x;
                    roomCenter.y = preRoomData.roomCenter.y + vertical * maxRoomHeight + vertical * roomDistance;

                    startX = roomCenter.x - randomWidth / 2;
                    startY = roomCenter.y - vertical * randomHeight / 2;
                }

            } else {
                roomCenter = new Vector2Int (startX + randomWidth / 2, startY + randomHeight / 2);
            }

            if (horizontal == 0) {
                horizontal = 1;
            }
            if (vertical == 0) {
                vertical = 1;
            }

            for (int j = 0; j < randomHeight; j++) {
                for (int k = 0; k < randomWidth; k++) {
                    // 绘制地板
                    floorTilemap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), floor);

                    // 绘制墙体
                    if (j == 0 || j == randomHeight - 1) {
                        wallTileMap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), wall);
                    } else {
                        if (k == 0 || k == randomWidth - 1) {
                            wallTileMap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), wall);
                        }
                    }
                }
            }

            // 房间数据的赋值
            curRoomData.roomWidth = randomWidth;
            curRoomData.roomHeight = randomHeight;
            curRoomData.roomCenter = roomCenter;
        }
    }

    private void drawPassway () {
        int horizontal = 0;
        int vertical = 0;

        for (int i = 1; i < this.roomOrderList.Count; i++) {
            Vector2Int curRoomVec = this.roomOrderList[i];
            RoomData curRoomData = this.roomDic[curRoomVec];

            RoomData preRoomData = this.roomDic[curRoomData.preRoomPoint];

            // 根据房间在地图中的位置坐标，判断方向。
            horizontal = curRoomData.roomInMapPos.x - preRoomData.roomInMapPos.x;
            vertical = curRoomData.roomInMapPos.y - preRoomData.roomInMapPos.y;

            if (horizontal != 0) {
                int direct = horizontal / Mathf.Abs (horizontal);
                Vector2Int preRoomCenter = preRoomData.roomCenter;
                Vector2Int rightStart = new Vector2Int (preRoomCenter.x + direct * preRoomData.roomWidth / 2, preRoomCenter.y + this.passwayHeight / 2);

                Vector2Int curRoomCenter = curRoomData.roomCenter;
                Vector2Int rightEnd = new Vector2Int (curRoomCenter.x - direct * curRoomData.roomWidth / 2, curRoomCenter.y + this.passwayHeight / 2);

                int distance = Mathf.Abs (rightEnd.x - rightStart.x) + 1;
                for (int k = 0; k < this.passwayHeight; k++) {
                    for (int j = 0; j < distance; j++) {
                        this.floorTilemap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), floor);
                        if (k == 0 || k == this.passwayHeight - 1) {
                            this.wallTileMap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), wall);
                        }
                    }
                }
            }

            if (vertical != 0) {
                int direct = vertical / Mathf.Abs (vertical);
                Vector2Int preRoomCenter = preRoomData.roomCenter;
                Vector2Int upStart = new Vector2Int (preRoomCenter.x + this.passwayHeight / 2, preRoomCenter.y + direct * preRoomData.roomHeight / 2);

                Vector2Int curRoomCenter = curRoomData.roomCenter;
                Vector2Int downEnd = new Vector2Int (curRoomCenter.x + this.passwayHeight / 2, curRoomCenter.y - direct * curRoomData.roomHeight / 2);

                int distance = Mathf.Abs (downEnd.y - upStart.y) + 1;
                for (int k = 0; k < distance; k++) {
                    for (int j = 0; j < this.passwayHeight; j++) {
                        this.floorTilemap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), floor);
                        if (j == 0 || j == this.passwayHeight - 1) {
                            this.wallTileMap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), wall);
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

        RoomData startRoomData = new RoomData (nowPoint, Vector2Int.one * -1);
        roomDic.Add (nowPoint, startRoomData);

        Vector2Int connectPoint = nowPoint;

        while (curRoomCount < roomCount) {
            nowPoint = getNextPoint (nowPoint, mapRow, mapColumn);
            if (map[nowPoint.x, nowPoint.y] == 1) {
                connectPoint = nowPoint;
                continue;
            }

            map[nowPoint.x, nowPoint.y] = 1;
            curRoomCount++;
            roomOrderList.Add (nowPoint);

            RoomData roomData = new RoomData (nowPoint, connectPoint);
            roomDic.Add (nowPoint, roomData);

            connectPoint = nowPoint;
        }

        // FIXME: 测试逻辑等待删除
        for (var i = 0; i < this.roomOrderList.Count; i++) {
            Vector2Int point = this.roomOrderList[i];
            Debug.Log ("curRoomPoint: " + this.roomDic[point].roomInMapPos + " preRoomPoint: " + this.roomDic[point].preRoomPoint);
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
        this.floorTilemap.ClearAllTiles ();
        this.roomOrderList.Clear ();

        this.startRoomIndex = this.endRoomIndex = Vector2Int.zero;
    }
}