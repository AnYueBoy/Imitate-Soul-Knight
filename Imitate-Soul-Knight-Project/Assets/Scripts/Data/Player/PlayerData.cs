using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BaseData {

    public bool isNewPlayer = true;

    public int chapter = 1;

    public int level = 1;

    /// <summary>
    /// 当前角色id 
    /// </summary>
    public int curRoleId = 1;

    public int curWeaponId = 1001;
}