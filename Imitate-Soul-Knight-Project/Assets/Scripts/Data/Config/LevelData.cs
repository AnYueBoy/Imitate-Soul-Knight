/*
 * @Author: l hy 
 * @Date: 2021-10-16 16:23:55 
 * @Description: 关卡数据
 */

using System.Collections.Generic;

public class LevelData : BaseData {

	public int chapter; // 章节
	public int level; // 关卡
	public int mapRowCount; // 地图行数量
	public int mapColumnCount; // 地图列数量
	public int passwayHeight; // 通道高度
	public int roomDistance; // 房间距离
	public int minRoomWidth; // 房间最小宽度
	public int maxRoomWidth; // 房间最大宽度
	public int minRoomHeight; // 房间最小高度
	public int maxRoomHeight; // 房间最大高度
	public int totalRoomCount; // 总房间数
	public List<int> enemyList; // 本关包含的敌人
	public int boss; // 本关boss
	public int specialRoomCount; // 特殊房间数
	public List<int> dropWeapon; // 掉落武器
}