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

    public void localUpdate (float dt) {
        this.move (dt);
    }

    protected virtual void OnTriggerEnter2D (Collider2D other) {
    }

    protected virtual void move (float dt) {

    }

}