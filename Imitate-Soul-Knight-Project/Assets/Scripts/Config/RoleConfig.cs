/*
 * @Author: l hy 
 * @Date: 2021-11-15 08:29:28 
 * @Description: 角色配置
 */
using System.Collections.Generic;

public class RoleConfig : IConfig {

    public List<RoleConfigData> roleList = new List<RoleConfigData> ();

    private Dictionary<int, RoleConfigData> roleConfigDic = new Dictionary<int, RoleConfigData> ();

    public RoleConfigData getRoleConfigById (int id) {
        return this.roleConfigDic[id];
    }

    public void convertData () {
        foreach (RoleConfigData roleConfigData in roleList) {
            this.roleConfigDic.Add (roleConfigData.id, roleConfigData);
        }
    }
}