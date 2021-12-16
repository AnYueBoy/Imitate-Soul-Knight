/*
 * @Author: l hy 
 * @Date: 2021-12-16 13:31:08 
 * @Description: 近战武器
 */

using UFramework;

public class MeleeWeapon : BaseWeapon {
    public override void attack (float args) {
        if (this.attackTimer < this.attackInterval) {
            return;
        }

        float weaponConsumeValue = this.weaponConfigData.mpConsume;
        float curMp = ModuleManager.instance.playerManager.getCurMp ();
        if (curMp < weaponConsumeValue) {
            return;
        }

        ModuleManager.instance.playerManager.consumeMp (weaponConsumeValue);

        this.attackTimer = 0;

        // TODO: 攻击伤害逻辑

    }

    public override void localUpdate (float dt) {
        this.attackTimer += dt;
    }
}