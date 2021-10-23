/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:33:18 
 * @Description: 子弹基类
 * @Last Modified by: l hy
 * @Last Modified time: 2021-10-23 08:49:12
 */

using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    public BulletData bulletData;

    public virtual void init (BulletData bulletData) {
        this.bulletData = bulletData;
    }

    public void localUpdate (float dt) {
        this.move (dt);
    }

    private void OnTriggerEnter2D (Collider2D other) {
        this.triggerHandler ();
    }

    protected virtual void triggerHandler () {
        this.bulletData.isDie = true;
    }

    protected virtual void move (float dt) {

    }

}