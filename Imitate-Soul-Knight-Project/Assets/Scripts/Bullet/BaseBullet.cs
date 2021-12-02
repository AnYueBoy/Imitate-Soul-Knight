/*
 * @Author: l hy 
 * @Date: 2021-10-22 21:33:18 
 * @Description: 子弹基类
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-12 12:19:48
 */

using UnityEngine;

public class BaseBullet : MonoBehaviour {

    public BulletData bulletData;

    public virtual void init (BulletData bulletData) {
        this.bulletData = bulletData;
    }

    protected RaycastHit2D checkRayCast (float step, Vector2 checkDir, Vector2 size, float angle) {
        RaycastHit2D raycastInfo = Physics2D.BoxCast (
            this.transform.position,
            size,
            angle,
            checkDir,
            step,
            1 << LayerMask.NameToLayer (LayerGroup.enemy) | 1 << LayerMask.NameToLayer (LayerGroup.player) | 1 << LayerMask.NameToLayer (LayerGroup.block));

        return raycastInfo;
    }

    public void localUpdate (float dt) {
        this.move (dt);
    }

    protected virtual void move (float dt) {
        float moveSpeed = this.bulletData.moveSpeed;
        float moveDir = this.bulletData.moveDir;

        float step = dt * moveSpeed;
        float angle = this.transform.eulerAngles.z;

        // 矩形射线检测
        float radValue = angle * Mathf.PI / 180;
        Vector2 checkDir = new Vector2 (Mathf.Cos (radValue) * moveDir, Mathf.Sin (radValue) * moveDir);
        checkDir = checkDir.normalized;
        RaycastHit2D raycastInfo = this.checkRayCast (step, checkDir, new Vector2 (0.52f, 0.18f), angle);
        if (raycastInfo) {
            this.triggerHandler (raycastInfo);
        }

        this.transform.Translate (new Vector3 (moveDir * step, 0, 0));
    }

    protected virtual void triggerHandler (RaycastHit2D raycastInfo) {

    }

}