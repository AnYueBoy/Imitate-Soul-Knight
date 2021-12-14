/*
 * @Author: l hy 
 * @Date: 2021-12-14 12:25:32 
 * @Description: 方块
 */

using UFramework;
using UFramework.GameCommon;

public class BoxBlock : DestructibleBlock {
    public override void destroyItem () {
        ObjectPool.instance.returnInstance (this.gameObject);

        // TODO：被破坏特效
        // TODO: 更新寻路障碍信息
        // 生成能量item
        ModuleManager.instance.itemManager.spawnItem (this.transform.position, ItemIdEnum.MP_ITEM);
    }
}