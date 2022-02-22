using System.Collections.Generic;
using UFramework.Core;
using UFramework.GameCommon;
using UnityEngine;

public class BulletManager : IBulletManager {

    private HashSet<BaseBullet> bulletSet = new HashSet<BaseBullet> ();

    private List<BaseBullet> removeBullets = new List<BaseBullet> ();

    public void LocalUpdate (float dt) {
        foreach (BaseBullet bullet in bulletSet) {
            if (bullet.bulletData.isDie) {
                this.removeBullets.Add (bullet);
            } else {
                bullet.localUpdate (dt);
            }
        }

        foreach (BaseBullet removeBullet in removeBullets) {
            this.bulletSet.Remove (removeBullet);
            App.Make<IObjectPool> ().ReturnInstance (removeBullet.gameObject);
        }

        if (removeBullets.Count > 0) {
            this.removeBullets.Clear ();
        }

    }

    public void SpawnBullet (
        Vector3 position,
        Vector3 eulerAngles,
        float bulletDir,
        string layer,
        string bulletUrl,
        float bulletSpeed,
        float bulletDamage) {
        GameObject bulletPrefab = App.Make<IAssetsManager> ().GetAssetByUrlSync<GameObject> (bulletUrl);
        GameObject bulletNode = App.Make<IObjectPool> ().RequestInstance (bulletPrefab);

        bulletNode.transform.position = position;
        bulletNode.transform.eulerAngles = eulerAngles;
        bulletNode.transform.localScale = new Vector3 (bulletDir, 1, 1);
        bulletNode.transform.SetParent (App.Make<ITransfromManager> ().GoTransfrom);

        BaseBullet bullet = bulletNode.GetComponent<BaseBullet> ();
        this.bulletSet.Add (bullet);

        BulletData bulletData = new BulletData (bulletDir, bulletSpeed, bulletDamage, layer);
        bullet.init (bulletData);
    }
}