using System.Collections;
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;

public class BulletManager {

    private HashSet<BaseBullet> bulletSet = new HashSet<BaseBullet> ();

    private List<BaseBullet> removeBullets = new List<BaseBullet> ();

    public void localUpdate (float dt) {
        foreach (BaseBullet bullet in bulletSet) {
            if (bullet.bulletData.isDie) {
                this.removeBullets.Add (bullet);
            } else {
                bullet.localUpdate (dt);
            }
        }

        foreach (BaseBullet removeBullet in removeBullets) {
            this.bulletSet.Remove (removeBullet);
            ObjectPool.instance.returnInstance (removeBullet.gameObject);
        }

        if (removeBullets.Count > 0) {
            this.removeBullets.Clear ();
        }

    }

    public void spawnBullet (string bulletUrl, Transform bulletTrans, float bulletSpeed, Vector3 bulletDir) {
        GameObject bulletPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (bulletUrl);
        GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);
        bulletNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);
        bulletNode.transform.position = bulletTrans.position;
        bulletNode.transform.eulerAngles = bulletTrans.eulerAngles;

        BaseBullet bullet = bulletNode.GetComponent<BaseBullet> ();
        this.bulletSet.Add (bullet);

        BulletData bulletData = new BulletData (bulletDir, bulletSpeed);
        bullet.init (bulletData);
    }
}