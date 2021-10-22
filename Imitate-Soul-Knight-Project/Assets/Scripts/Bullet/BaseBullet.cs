/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:33:18 
 * @Description: 子弹基类
 * @Last Modified by: l hy
 * @Last Modified time: 2021-10-22 22:17:27
 */

using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    [HideInInspector]
    public bool isDie = false;

    public virtual void init (float moveSpeed) {
        this.isDie = false;
    }

    public void localUpdate (float dt) {
        this.move (dt);
    }

    private void OnCollisionEnter2D (Collision2D other) {
        this.collisionHandler ();
    }

    protected virtual void collisionHandler () {
        this.isDie = true;
    }

    protected virtual void move (float dt) {

    }

}