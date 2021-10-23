/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:39:01 
 * @Description: 普通子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-10-23 08:50:16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet {
    private Vector2 moveDir;

    protected override void move (float dt) {
        float moveSpeed = this.bulletData.moveSpeed;
        float moveDir = this.bulletData.moveDir;
        // translate 不是简单的坐标值增加，而是对当前物体向某个方向移动一块距离 
        // （x,y,z）为当前的方向性和大小，unity对方向和大小确认后，在当前向量的基础上，进行增加
        this.transform.Translate (new Vector3 (moveDir * dt * moveSpeed, 0, 0));
    }
}