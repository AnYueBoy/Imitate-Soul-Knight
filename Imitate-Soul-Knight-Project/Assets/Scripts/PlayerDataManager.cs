/*
 * @Author: l hy 
 * @Date: 2021-10-17 14:05:02 
 * @Description: 玩家数据管理
 * @Last Modified by: l hy
 * @Last Modified time: 2021-10-17 14:30:12
 */

using System.IO;
using LitJson;
using UFramework.GameCommon;
using UnityEngine;

public class PlayerDataManager {
    private PlayerData playerData = new PlayerData ();
    private readonly string playerDataUrl = "PlayerData";

    public void init () {
        this.parseData ();
    }

    #region  数据读取与保存
    private void parseData () {
        TextAsset jsonAsset = AssetsManager.instance.getAssetByUrlSync<TextAsset> (playerDataUrl);
        if (jsonAsset == null) {
            return;
        }
        string context = jsonAsset.text;
        if (string.IsNullOrEmpty (context)) {
            return;
        }

        this.playerData = JsonMapper.ToObject<PlayerData> (context);
        this.playerData.isNewPlayer = false;
    }

    public void saveData () {
        string playerDataStr = JsonMapper.ToJson (this.playerData);
        string filePath = Application.dataPath + "/Resources/" + this.playerDataUrl + ".json";

        StreamWriter sw = new StreamWriter (filePath);
        sw.Write (playerDataStr);
        sw.Close ();
    }

    #endregion


    #region  数据访问


    #endregion
}