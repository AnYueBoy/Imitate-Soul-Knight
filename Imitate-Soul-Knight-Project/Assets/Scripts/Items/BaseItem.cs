/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:38:28 
 * @Description: {} 
 */

using UFramework;
using UnityEngine;
public class BaseItem : MonoBehaviour {
    public virtual void localUpdate (float dt) {

    }

    public float getSelfToPlayerDis () {
        Transform playerTrans = ModuleManager.instance.playerManager.getPlayerTrans ();
        float distance = (this.transform.position - playerTrans.position).magnitude;
        return distance;
    }
}