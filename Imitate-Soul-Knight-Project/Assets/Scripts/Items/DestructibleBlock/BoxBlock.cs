/*
 * @Author: l hy 
 * @Date: 2021-12-14 12:25:32 
 * @Description: 方块
 */

using System;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class BoxBlock : DestructibleBlock {

    private Action<Vector3> callback;
    public override void init (Action<Vector3> callback) {
        this.callback = callback;
    }
    public override void destroyItem () {
        this.callback?.Invoke (this.transform.position);
        this.callback = null;
        
        ObjectPool.instance.returnInstance (this.gameObject);

        // TODO：被破坏特效
        // TODO: 更新寻路障碍信息
        // 生成能量item
        ModuleManager.instance.itemManager.spawnItem (this.transform.position, ItemIdEnum.MP_ITEM);
    }

}