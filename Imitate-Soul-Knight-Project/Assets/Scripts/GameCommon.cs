/*
 * @Author: l hy 
 * @Date: 2021-05-18 16:05:44 
 * @Description: GameCommon
 */

using UFramework;
using UnityEngine;
public class GameCommon : MonoBehaviour {

    private float saveTimer = 0;

    private readonly float saveInterval = 3f;

    private void Update () {
        this.saveDataByFixedTime (Time.deltaTime);
    }
    private void saveDataByFixedTime (float dt) {
        this.saveTimer += dt;
        if (this.saveTimer < this.saveInterval) {
            return;
        }

        this.saveTimer = 0;
        ModuleManager.instance.playerDataManager.saveData ();
    }

    private void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            this.onHideCall ();
        } else {
            this.onShowCall ();
        }
    }

    private void onHideCall () {
        ModuleManager.instance.playerDataManager.saveData ();
    }

    private void onShowCall () {

    }
}