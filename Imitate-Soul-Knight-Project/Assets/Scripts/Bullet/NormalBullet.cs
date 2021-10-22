/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:39:01 
 * @Description: 普通子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-10-22 22:47:53
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet {
    private float moveSpeed;

    public override void init (float moveSpeed) {
        base.init (moveSpeed);
        this.moveSpeed = moveSpeed;

    }
    protected override void move (float dt) {
        base.move (dt);
        this.transform.Translate (new Vector3 (moveSpeed * dt, 0, 0));
    }
}