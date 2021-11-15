/*
 * @Author: l hy 
 * @Date: 2021-11-15 08:40:14 
 * @Description: 战斗角色数据
 */

using UFramework;

public class BattleRoleData : BaseData {
    public int roleId;

    public float curHp;

    public float curMaxHp;

    public float curMp;

    public float curMaxMp;

    public float curArmor;

    public float curMaxArmor;

    public float curCriticalHit;

    public BattleRoleData (int roleId) {
        this.roleId = roleId;

        RoleConfigData roleConfigData = ModuleManager.instance.configManager.roleConfig.getRoleConfigById (this.roleId);

        this.curHp = this.curMaxHp = roleConfigData.hp;

        this.curMp = this.curMaxMp = roleConfigData.mp;

        this.curArmor = this.curMaxArmor = roleConfigData.armor;

        this.curCriticalHit = roleConfigData.criticalHit;
    }
}