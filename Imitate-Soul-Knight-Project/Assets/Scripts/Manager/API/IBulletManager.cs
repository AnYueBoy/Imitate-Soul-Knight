using UnityEngine;
public interface IBulletManager {
    void LocalUpdate (float deltaTime);

    void SpawnBullet (Vector3 position, Vector3 eulerAngle, float bulletDir, string layer, string bulletUrl, float bulletSpeed, float bulletDamage);
}