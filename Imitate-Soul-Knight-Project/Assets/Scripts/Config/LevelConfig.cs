using System.Collections.Generic;
using LitJson;
using UFramework.GameCommon;
using UnityEngine;
/*
 * @Author: l hy 
 * @Date: 2021-10-16 16:33:01 
 * @Description: 章节关卡配置
 */

public class LevelConfig {

	public List<ChapterData> chapterList = new List<ChapterData> ();

	public LevelData getLevelDataByChapterLevel (int chapter, int level) {
		ChapterData chapterData = this.chapterList[chapter - 1];
		LevelData levelData = chapterData.levelList[level - 1];
		return levelData;
	}
}