/*
 * @Author: l hy 
 * @Date: 2021-11-24 17:18:54 
 * @Description: 传送门
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : PassiveTriggerItem {
    private void OnEnable () {
        this.triggerDistance = 1.5f;
    }

    public override void triggerHandler () {
        base.triggerHandler ();

        Debug.Log ("进入下一关");
    }
}