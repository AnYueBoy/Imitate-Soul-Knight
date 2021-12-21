/*
 * @Author: l hy 
 * @Date: 2021-09-17 14:00:13 
 * @Description: 角色控制
 */
using System;
using DG.Tweening;
using UFramework;
using UnityEngine;

public class RoleControl : MonoBehaviour {

    [SerializeField] private Transform selfTrans;

    [SerializeField] private float checkDistance = 0.4f;

    public Transform rangeWeaponParent;
    public Transform meleeWeaponParent;

    private Transform rangeWeaponRotationTrans;
    private Transform meleeWeaponRotationTrans;

    private Animator animator;

    private Material hurtFlashMaterial;

    private void OnEnable () {
        this.animator = transform.GetComponent<Animator> ();
        this.hurtFlashMaterial = transform.GetComponent<SpriteRenderer> ().material;
    }

    public void localUpdate (float dt) {
        this.roleAni ();
        this.roleMove (dt);
        this.refreshWeaponRotate ();
    }

    private void roleMove (float dt) {
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        if (moveDir == Vector2.zero) {
            return;
        }

        if (moveDir.x != 0) {
            float horizontal = Mathf.Abs (moveDir.x) / moveDir.x;
            RaycastHit2D raycastHitInfo = Physics2D.Raycast (
                selfTrans.position,
                new Vector2 (horizontal, 0),
                checkDistance,
                1 << LayerMask.NameToLayer (LayerGroup.block) | 1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));

            if (raycastHitInfo) {
                moveDir.x = 0;
            }
        }

        if (moveDir.y != 0) {
            float vertical = Mathf.Abs (moveDir.y) / moveDir.y;
            RaycastHit2D raycastHitInfo = Physics2D.Raycast (
                selfTrans.position,
                new Vector2 (0, vertical),
                checkDistance,
                1 << LayerMask.NameToLayer (LayerGroup.block) | 1 << LayerMask.NameToLayer (LayerGroup.destructibleBlock));
            if (raycastHitInfo) {
                moveDir.y = 0;
            }
        }

        transform.Translate (moveDir * dt * ConstValue.moveSpeed);

    }

    private void refreshWeaponRotate () {
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        this.weaponRotate (new Vector2 (transform.localScale.x, 0), moveDir);
    }

    private void weaponRotate (Vector2 refer, Vector2 moveDir) {
        BaseEnemy closetEnemy = ModuleManager.instance.mapManager.getClosetEnemy ();
        // 附近有敌人则武器优先朝向敌人
        if (closetEnemy != null) {
            moveDir = (closetEnemy.transform.position - this.transform.position).normalized;
        }

        if (moveDir == Vector2.zero) {
            return;
        }

        // refer 为参考正方向
        float rotationAngle = Vector2.SignedAngle (refer, moveDir);
        Vector3 endEulerAngles = new Vector3 (0, 0, rotationAngle);
        if (this.rangeWeaponRotationTrans != null) {
            this.rangeWeaponRotationTrans.eulerAngles = endEulerAngles;
        }
        if (this.meleeWeaponRotationTrans != null) {
            this.meleeWeaponRotationTrans.eulerAngles = endEulerAngles;
        }

        if (moveDir.x != 0) {
            float sign = moveDir.x / Mathf.Abs (moveDir.x);
            transform.localScale = new Vector3 (sign, 1, 1);
        }
    }

    public void setRangeWeaponRotaTrans (Transform rangeTrans) {
        this.rangeWeaponRotationTrans = rangeTrans;
    }
    public void setMeleeWeaponRotaTrans (Transform meleeTrans) {
        this.meleeWeaponRotationTrans = meleeTrans;
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

    public void hurtEffect (float time, Action callback) {
        Color originColor = new Color (0, 0, 0, 0);
        DOTween
            .To (
                () => {
                    return originColor;
                },
                (value) => {
                    originColor = value;
                    this.hurtFlashMaterial.SetColor ("_HurtColor", originColor);
                },
                new Color (1, 1, 1, 0),
                time / 2)
            .OnComplete (() => {
                DOTween.To (
                        () => {
                            return originColor;
                        },
                        (value) => {
                            originColor = value;
                            this.hurtFlashMaterial.SetColor ("_HurtColor", originColor);
                        },
                        new Color (0, 0, 0, 0),
                        time / 2)
                    .OnComplete (() => {
                        callback?.Invoke ();
                    });
            });
    }

    private void OnDisable () {

    }
}