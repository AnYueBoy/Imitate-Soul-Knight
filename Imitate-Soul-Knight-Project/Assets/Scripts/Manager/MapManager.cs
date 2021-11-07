/*
 * @Author: l hy 
 * @Date: 2021-09-29 08:40:01 
 * @Description: 地图生成管理
 */
using System.Collections.Generic;
using System.Linq;
using UFramework;
using UFramework.FrameUtil;
using UFramework.GameCommon;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    public Tilemap floorTilemap;

    public Tilemap wallTileMap;

    #region  房间参数
    private int mapRowCount;

    private int mapColumnCount;

    private int totalRoomCount;

    private int passwayHeight;

    private int roomDistance;

    private int minRoomWidth;

    private int maxRoomWidth;

    private int minRoomHeight;

    private int maxRoomHeight;

    private int specialRoomCount;

    #endregion

    private Dictionary<Vector2Int, Room> roomDic = new Dictionary<Vector2Int, Room> ();

    private List<Vector2Int> roomOrderList = new List<Vector2Int> ();

    private TileBase floorTile;
    private TileBase doorOpenTile;
    private TileBase wallTile;

    private LevelData levelData;

    public void init () {
        this.generateMapClick ();
    }

    private void initMapParams () {
        int chapter = ModuleManager.instance.playerDataManager.getCurChapter ();
        int level = ModuleManager.instance.playerDataManager.getCurLevel ();
        this.levelData = ModuleManager.instance.configManager.levelConfig.getLevelDataByChapterLevel (chapter, level);

        this.mapRowCount = levelData.mapRowCount;
        this.mapColumnCount = levelData.mapColumnCount;
        this.passwayHeight = levelData.passwayHeight;
        this.roomDistance = levelData.roomDistance;
        this.minRoomWidth = levelData.minRoomWidth;
        this.maxRoomWidth = levelData.maxRoomWidth;
        this.minRoomHeight = levelData.minRoomHeight;
        this.maxRoomHeight = levelData.maxRoomHeight;
        this.totalRoomCount = levelData.totalRoomCount;
        this.specialRoomCount = levelData.specialRoomCount;
    }

    private void initMapTile () {
        floorTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.floorTile);
        wallTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.wallTile);
        doorOpenTile = AssetsManager.instance.getAssetByUrlSync<TileBase> (MapAssetsUrl.doorOpenTile);
    }

    public void generateMapClick () {
        this.initMapTile ();
        this.initMapParams ();
        this.resetMap ();
        this.generateMapRoom ();
        this.drawRoom ();
        this.drawPassway ();
    }

    private void generateMapRoom () {
        int spawnRoomCount = Mathf.Min (mapRowCount * mapColumnCount, totalRoomCount);
        this.getRoomMap (mapRowCount, mapColumnCount, spawnRoomCount);
    }

    public void localUpdate (float dt) {
        foreach (Room room in this.roomDic.Values) {
            if (room == null) {
                continue;
            }

            room.localUpdate (dt);
        }
    }

    private void drawRoom () {
        //  随机房间大小
        int startX = 0;
        int startY = 0;
        Vector2Int curRoomPoint = Vector2Int.zero;
        Vector2Int preRoomPoint = Vector2Int.zero;
        Vector2Int roomCenter = Vector2Int.zero;

        for (int i = 0; i < this.roomOrderList.Count; i++) {
            curRoomPoint = this.roomOrderList[i];
            RoomData curRoomData = this.roomDic[curRoomPoint].roomData;

            int randomWidth = CommonUtil.getOddNumber (minRoomWidth, maxRoomWidth);
            int randomHeight = CommonUtil.getOddNumber (minRoomHeight, maxRoomHeight);

            int horizontal = 1;
            int vertical = 1;
            if (i > 0) {
                preRoomPoint = curRoomData.preRoomPoint;
                RoomData preRoomData = this.roomDic[preRoomPoint].roomData;
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
                    floorTilemap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), floorTile);

                    // 绘制墙体
                    if (j == 0 || j == randomHeight - 1) {
                        wallTileMap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), wallTile);
                    } else {
                        if (k == 0 || k == randomWidth - 1) {
                            wallTileMap.SetTile (new Vector3Int (startX + k * horizontal, startY + j * vertical, 0), wallTile);
                        }
                    }
                }
            }

            // 房间数据的赋值
            curRoomData.roomWidth = randomWidth;
            curRoomData.roomHeight = randomHeight;
            curRoomData.roomCenter = roomCenter;

            // 房间初始化。计算房间内相关数据
            this.roomDic[curRoomPoint].init ();
        }
    }

    private void drawPassway () {
        int horizontal = 0;
        int vertical = 0;

        for (int i = 1; i < this.roomOrderList.Count; i++) {
            Vector2Int curRoomVec = this.roomOrderList[i];
            RoomData curRoomData = this.roomDic[curRoomVec].roomData;

            RoomData preRoomData = this.roomDic[curRoomData.preRoomPoint].roomData;

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
                        this.floorTilemap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), floorTile);
                        if (k == 0 || k == this.passwayHeight - 1) {
                            this.wallTileMap.SetTile (new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0), wallTile);
                        } else {
                            if (j == 0 || j == distance - 1) {
                                // 设置门
                                Vector3Int doorTilePoint = new Vector3Int (rightStart.x + direct * j, rightStart.y - k, 0);
                                this.wallTileMap.SetTile (doorTilePoint, doorOpenTile);

                                if (j == 0) {
                                    preRoomData.doorList.Add (doorTilePoint);
                                }
                                if (j == distance - 1) {
                                    curRoomData.doorList.Add (doorTilePoint);
                                }
                            }
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
                        this.floorTilemap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), floorTile);
                        if (j == 0 || j == this.passwayHeight - 1) {
                            this.wallTileMap.SetTile (new Vector3Int (upStart.x - j, upStart.y + direct * k, 0), wallTile);
                        } else {
                            if (k == 0 || k == distance - 1) {
                                // 设置门
                                Vector3Int doorTilePoint = new Vector3Int (upStart.x - j, upStart.y + direct * k, 0);
                                this.wallTileMap.SetTile (doorTilePoint, doorOpenTile);
                                if (k == 0) {
                                    preRoomData.doorList.Add (doorTilePoint);
                                }

                                if (k == distance - 1) {
                                    curRoomData.doorList.Add (doorTilePoint);
                                }
                            }
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

        int curRoomCount = 1;

        int[, ] map = new int[mapRow, mapColumn];
        map[nowPoint.x, nowPoint.y] = 1;

        roomOrderList.Add (nowPoint);

        RoomData startRoomData = new RoomData (nowPoint, Vector2Int.one * -1, RoomTypeEnum.BORN, levelData);

        Room startRoom = new Room (startRoomData);
        roomDic.Add (nowPoint, startRoom);

        Vector2Int connectPoint = nowPoint;

        List<RoomTypeEnum> roomTypeList = this.getRandomRoomType ();

        while (curRoomCount < roomCount) {
            nowPoint = getNextPoint (nowPoint, mapRow, mapColumn);
            if (map[nowPoint.x, nowPoint.y] == 1) {
                connectPoint = nowPoint;
                continue;
            }

            map[nowPoint.x, nowPoint.y] = 1;
            curRoomCount++;
            roomOrderList.Add (nowPoint);

            // 获取随机的房间类型
            RoomTypeEnum roomType = RoomTypeEnum.BATTLE;
            if (curRoomCount >= roomCount) {
                roomType = RoomTypeEnum.PORTAL;
            } else {
                roomType = roomTypeList[curRoomCount - 2];
            }
            RoomData roomData = new RoomData (nowPoint, connectPoint, roomType, levelData);
            Room room = new Room (roomData);
            roomDic.Add (nowPoint, room);

            connectPoint = nowPoint;
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
        this.wallTileMap.ClearAllTiles ();
        this.roomOrderList.Clear ();
    }

    private List<RoomTypeEnum> getRandomRoomType () {
        List<RoomTypeEnum> roomTypeList = new List<RoomTypeEnum> ();
        for (int i = 0; i < specialRoomCount; i++) {
            RoomTypeEnum roomType = (RoomTypeEnum) CommonUtil.getRandomValue ((int) RoomTypeEnum.CHEST, (int) RoomTypeEnum.BLESS);
            roomTypeList.Add (roomType);
        }

        for (var i = 0; i < totalRoomCount - 2 - specialRoomCount; i++) {
            roomTypeList.Add (RoomTypeEnum.BATTLE);
        }

        CommonUtil.confusionElement<RoomTypeEnum> (roomTypeList);
        return roomTypeList;
    }

    /// <summary>
    /// 返回tile的世界坐标，中心在tile中心
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public Vector3 cellToWorldPos (Vector3Int tilePos) {
        Vector3 worldPosLeftDown = this.floorTilemap.CellToWorld (tilePos);
        Vector3 worldPosCenter = new Vector3 (worldPosLeftDown.x + this.wallTileMap.cellSize.x / 2, worldPosLeftDown.y + this.wallTileMap.cellSize.y / 2, 0);
        return worldPosCenter;
    }
}