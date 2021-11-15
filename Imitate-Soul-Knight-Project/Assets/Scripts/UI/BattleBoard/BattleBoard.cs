/*
 * @Author: l hy 
 * @Date: 2021-11-15 12:18:49 
 * @Description: 战斗界面
 */

using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class BattleBoard : BaseUI {

    [SerializeField]
    private RoleInfo roleInfo;

    public override void onShow (params object[] args) {

    }

    private void Update () {
        this.roleInfo.localUpdate ();
    }
}