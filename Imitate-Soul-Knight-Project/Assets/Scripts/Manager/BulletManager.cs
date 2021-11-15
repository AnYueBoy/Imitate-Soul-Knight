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

    public void spawnBullet (
        Vector3 position,
        Vector3 eulerAngles,
        float bulletDir,
        string tag,
        WeaponConfigData weaponConfigData) {
        string bulletUrl = weaponConfigData.bulletUrl;
        GameObject bulletPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (bulletUrl);
        GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);

        bulletNode.transform.position = position;
        bulletNode.transform.eulerAngles = eulerAngles;
        bulletNode.transform.localScale = new Vector3 (bulletDir, 1, 1);
        bulletNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        bulletNode.tag = tag;

        BaseBullet bullet = bulletNode.GetComponent<BaseBullet> ();
        this.bulletSet.Add (bullet);

        BulletData bulletData = new BulletData (bulletDir, weaponConfigData.bulletSpeed, weaponConfigData.damage, tag);
        bullet.init (bulletData);
    }

    public void spawnBullet (
        Vector3 position,
        Vector3 eulerAngles,
        float bulletDir,
        string tag,
        string bulletUrl,
        float bulletSpeed,
        float bulletDamage) {
        GameObject bulletPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (bulletUrl);
        GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);

        bulletNode.transform.position = position;
        bulletNode.transform.eulerAngles = eulerAngles;
        bulletNode.transform.localScale = new Vector3 (bulletDir, 1, 1);
        bulletNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        bulletNode.tag = tag;

        BaseBullet bullet = bulletNode.GetComponent<BaseBullet> ();
        this.bulletSet.Add (bullet);

        BulletData bulletData = new BulletData (bulletDir, bulletDamage, bulletSpeed, tag);
        bullet.init (bulletData);
    }
}