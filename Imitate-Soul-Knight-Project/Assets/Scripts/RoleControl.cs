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
        this.roleAni ();
        this.roleMove (Time.deltaTime);
    }

    private void roleMove (float dt) {
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        if (moveDir == Vector2.zero) {
            return;
        }
        transform.Translate (moveDir * dt * moveSpeed);

        if (moveDir.x != 0) {
            float sign = moveDir.x / Mathf.Abs (moveDir.x);
            transform.localScale = new Vector3 (sign, 1, 1);
        }
    }

    private void roleAni () {
        AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo (0);

        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        if (moveDir == Vector2.zero) {
            bool inIdleState = animatorStateInfo.IsName ("Idle");
            if (!inIdleState) {
                this.animator.SetBool ("IsMove", false);
            }
            return;
        }

        bool inMoveState = animatorStateInfo.IsName ("Move");
        if (!inMoveState) {
            this.animator.SetBool ("IsMove", true);
        }

    }
}