using System.Collections.Generic;
using LitJson;
using UFramework.GameCommon;
using UnityEngine;
/*
 * @Author: l hy 
 * @Date: 2021-10-16 16:33:01 
 * @Description: 章节关卡配置
 */

public class LevelConfig : IConfig {

	public List<LevelData> chapterList = new List<LevelData> ();

	private Dictionary<int, Dictionary<int, LevelData>> levelDataDic = new Dictionary<int, Dictionary<int, LevelData>> ();

	public LevelData getLevelDataByChapterLevel (int chapter, int level) {
		Dictionary<int, LevelData> chapterDic = this.levelDataDic[chapter];
		return chapterDic[level];
	}

	public void convertData () {
		foreach (LevelData levelData in this.chapterList) {
			Dictionary<int, LevelData> levelDic = null;
			if (!this.levelDataDic.ContainsKey (levelData.chapter)) {
				levelDic = new Dictionary<int, LevelData> ();
				levelDic.Add (levelData.level, levelData);
				this.levelDataDic.Add (levelData.chapter, levelDic);
				continue;
			}

			levelDic = this.levelDataDic[levelData.chapter];
			levelDic.Add (levelData.level, levelData);
		}
	}
}