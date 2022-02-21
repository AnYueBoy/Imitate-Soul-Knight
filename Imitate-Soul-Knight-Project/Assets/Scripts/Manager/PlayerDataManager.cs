/*
 * @Author: l hy 
 * @Date: 2021-10-17 14:05:02 
 * @Description: 玩家数据管理
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-15 08:50:16
 */

using System.IO;
using LitJson;
using UFramework.Core;
using UFramework.GameCommon;
using UnityEngine;

public class PlayerDataManager : IPlayerDataManager {
    private PlayerData playerData = new PlayerData ();
    private readonly string playerDataUrl = "PlayerData";

    public void Init () {
        this.ParseData ();
    }

    #region  数据读取与保存
    private void ParseData () {
        TextAsset jsonAsset = App.Make<IAssetsManager> ().GetAssetByUrlSync<TextAsset> (playerDataUrl);
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

    private void SaveData () {
        string playerDataStr = JsonMapper.ToJson (this.playerData);
        string filePath = UnityEngine.Application.dataPath + "/Resources/" + this.playerDataUrl + ".json";

        StreamWriter sw = new StreamWriter (filePath);
        sw.Write (playerDataStr);
        sw.Close ();
    }

    #endregion

    private float saveTimer = 0;
    private readonly float saveInterval = 5f;

    public void SaveDataByFixedTime (float deltaTime) {
        if (this.saveTimer < this.saveInterval) {
            return;
        }

        this.saveTimer = 0;
        this.SaveData ();
    }

    private void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            OnAppHide ();
        } else {
            OnAppShow ();
        }
    }

    private void OnAppShow () {

    }

    private void OnAppHide () {
        this.SaveData ();
    }

    #region  数据访问

    public int CurRoleId {
        get {
            return this.playerData.curRoleId;
        }
    }

    public int CurWeaponId {
        get {
            return this.playerData.curWeaponId;

        }
    }
    #endregion
}