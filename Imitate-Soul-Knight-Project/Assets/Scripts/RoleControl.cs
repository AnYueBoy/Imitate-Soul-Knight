/*
 * @Author: l hy 
 * @Date: 2021-09-17 14:00:13 
 * @Description: 角色控制
 */
using System.Collections;
using System.Collections.Generic;
using UFramework;
using UnityEngine;

public class RoleControl : MonoBehaviour {

    private readonly float moveSpeed = 2;

    private Animator animator;

    private void OnEnable () {
        this.animator = transform.GetComponent<Animator> ();
    }

    private void Update () {
        this.roleMove (Time.deltaTime);
    }

    private void roleMove (float dt) {
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        if (moveDir == Vector2.zero) {
            return;
        }
        transform.Translate (moveDir * dt * moveSpeed);

        if (moveDir.x != 0) {
            transform.localScale = new Vector3 (moveDir.x, 1, 1);
        }

    }
}