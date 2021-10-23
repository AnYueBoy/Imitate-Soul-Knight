using System.Collections;
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class BulletManager {

    private HashSet<Bullet> bulletSet = new HashSet<Bullet> ();

    private List<Bullet> removeBullets = new List<Bullet> ();

    public void localUpdate (float dt) {
        foreach (Bullet bullet in bulletSet) {
            if (bullet.bulletData.isDie) {
                this.removeBullets.Add (bullet);
            } else {
                bullet.localUpdate (dt);
            }
        }

        foreach (Bullet removeBullet in removeBullets) {
            this.bulletSet.Remove (removeBullet);
            ObjectPool.instance.returnInstance (removeBullet.gameObject);
        }

        if (removeBullets.Count > 0) {
            this.removeBullets.Clear ();
        }

    }

    public void spawnBullet (string bulletUrl, Transform bulletTrans, float bulletSpeed, float bulletDir) {
        GameObject bulletPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (bulletUrl);
        GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);
        bulletNode.transform.position = bulletTrans.position;
        bulletNode.transform.eulerAngles = bulletTrans.eulerAngles;
        bulletNode.transform.localScale = new Vector3 (bulletDir, 1, 1);
        bulletNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        Bullet bullet = bulletNode.GetComponent<Bullet> ();
        this.bulletSet.Add (bullet);

        BulletData bulletData = new BulletData (bulletDir, bulletSpeed);
        bullet.init (bulletData);
    }
}