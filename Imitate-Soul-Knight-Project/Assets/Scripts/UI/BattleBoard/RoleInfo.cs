/*
 * @Author: l hy 
 * @Date: 2021-11-15 12:22:56 
 * @Description: 角色信息
 */
using System.Collections;
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
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

    private void OnEnable () {
        ListenerManager.instance.add (EventName.ATTRIBUTE_CHANGE, this, this.refreshInfo);
        this.refreshInfo ();
    }

    private void refreshInfo () {
        BattleRoleData battleRoleData = ModuleManager.instance.playerManager.battleRoleData;
        if (battleRoleData == null) {
            return;
        }

        // 信息刷新
        this.curHpText.text = battleRoleData.curHp + "/" + battleRoleData.curMaxHp;
        this.curHpImage.fillAmount = battleRoleData.curHp / battleRoleData.curMaxHp;

        this.curMpText.text = battleRoleData.curMp + "/" + battleRoleData.curMaxMp;
        this.curMpImage.fillAmount = battleRoleData.curMp / battleRoleData.curMaxMp;

        this.curArmorText.text = battleRoleData.curArmor + "/" + battleRoleData.curMaxArmor;
        this.curArmorImage.fillAmount = battleRoleData.curArmor / battleRoleData.curMaxArmor;
    }

    private void OnDisable () {
        ListenerManager.instance.removeAll (this);
    }
}