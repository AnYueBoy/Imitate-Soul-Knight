/*
 * @Author: l hy 
 * @Date: 2021-11-15 12:22:56 
 * @Description: 角色信息
 */
using System.Collections;
using System.Collections.Generic;
using UFramework;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfo : MonoBehaviour {
    [SerializeField]
    private Image curHpImage;
    [SerializeField]
    private Text curHpText;

    [SerializeField]
    private Image curMpImage;
    [SerializeField]
    private Text curMpText;

    [SerializeField]
    private Image curArmorImage;
    [SerializeField]
    private Text curArmorText;

    public void localUpdate () {
        this.refreshInfo ();
    }

    private void refreshInfo () {
        BattleRoleData battleRoleData = ModuleManager.instance.playerManager.battleRoleData;
        if (battleRoleData == null) {
            return;
        }
        // TODO: 信息刷新
    }
}