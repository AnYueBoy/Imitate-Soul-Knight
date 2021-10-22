/*
 * @Author: l hy 
 * @Date: 2021-09-17 14:00:13 
 * @Description: 角色控制
 */
using UFramework;
using UFramework.FrameUtil;
using UnityEngine;

public class RoleControl : MonoBehaviour {

    [SerializeField]
    private Transform selfTrans;

    [SerializeField]
    private float checkDistance = 0.4f;

    public LayerMask checkLayer;

    public SpriteRenderer weaponSprite;

    private readonly float moveSpeed = 15;

    private Animator animator;

    private void OnEnable () {
        this.animator = transform.GetComponent<Animator> ();
        ModuleManager.instance.inputManager.registerSwitch (this.switchWeapon);
        ModuleManager.instance.inputManager.registerSkill (this.useSkill);
        ModuleManager.instance.inputManager.registerAttack (this.attack);
    }

    public void localUpdate (float dt) {
        this.roleAni ();
        this.roleMove (dt);
    }

    private void OnDisable () {
        ModuleManager.instance.inputManager.unRegisterSwitch ();
        ModuleManager.instance.inputManager.unRegisterSkill ();
        ModuleManager.instance.inputManager.unRegisterAttack ();
    }

    private void roleMove (float dt) {
        Vector2 moveDir = ModuleManager.instance.inputManager.MoveDir;
        if (moveDir == Vector2.zero) {
            return;
        }

        if (moveDir.x != 0) {
            float horizontal = Mathf.Abs (moveDir.x) / moveDir.x;
            if (CommonUtil.ray2DCheck (selfTrans.position, new Vector2 (horizontal, 0), checkDistance, checkLayer)) {
                moveDir.x = 0;
            }
        }

        if (moveDir.y != 0) {
            float vertical = Mathf.Abs (moveDir.y) / moveDir.y;
            if (CommonUtil.ray2DCheck (selfTrans.position, new Vector2 (0, vertical), checkDistance, checkLayer)) {
                moveDir.y = 0;
            }
        }

        transform.Translate (moveDir * dt * moveSpeed);

        if (moveDir.x != 0) {
            float sign = moveDir.x / Mathf.Abs (moveDir.x);
            transform.localScale = new Vector3 (sign, 1, 1);
        }

        this.weaponRotate (new Vector2 (transform.localScale.x, 0), moveDir);
    }

    private void weaponRotate (Vector2 refer, Vector2 moveDir) {
        // refer 为参考正方向
        float rotationAngle = Vector2.SignedAngle (refer, moveDir);
        this.weaponSprite.transform.parent.eulerAngles = new Vector3 (0, 0, rotationAngle);
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

    private void switchWeapon () {

    }

    private void useSkill () {

    }

    private void attack () {
        Debug.Log ("attack");
    }
}