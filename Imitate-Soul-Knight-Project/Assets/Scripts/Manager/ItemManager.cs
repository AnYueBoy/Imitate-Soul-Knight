/*
 * @Author: l hy 
 * @Date: 2021-11-22 09:37:40 
 * @Description: 物品管理
 */
using System.Collections.Generic;
using UFramework;
using UFramework.GameCommon;
using UnityEngine;
public class ItemManager {

    public HashSet<BaseItem> itemSet = new HashSet<BaseItem> ();

    public void localUpdate (float dt) {
        foreach (var item in itemSet) {
            if (item == null || !item.gameObject.activeSelf) {
                continue;
            }

            item.localUpdate (dt);
        }
    }

    public BaseItem spawnItem (Vector3 pos, ItemIdEnum id) {
        string url = getItemPreUrl (id);
        GameObject itemPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (url);
        GameObject itemNode = ObjectPool.instance.requestInstance (itemPrefab);

        itemNode.transform.position = pos;
        itemNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        BaseItem item = itemNode.GetComponent<BaseItem> ();
        this.itemSet.Add (item);
        return item;
    }

    public BaseWeapon spawnWeapon (Vector3 pos, ItemIdEnum id) {
        string url = getItemPreUrl (id);
        GameObject itemPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (url);
        GameObject itemNode = ObjectPool.instance.requestInstance (itemPrefab);

        itemNode.transform.position = pos;
        itemNode.transform.SetParent (ModuleManager.instance.gameObjectTrans);

        BaseItem item = itemNode.GetComponent<BaseItem> ();
        this.itemSet.Add (item);

        BaseWeapon weapon = itemNode.GetComponent<BaseWeapon> ();
        weapon.init (id);
        return weapon;
    }

    public static string getItemPreUrl (ItemIdEnum id) {
        int idNumber = (int) id;
        int preId = idNumber / 1000;
        switch (preId) {
            case 1:
                return "Items/Weapon/" + idNumber;
            case 2:
                return "Items/Chest/" + idNumber;
            case 3:
                return "Items/Coin/" + idNumber;
            case 4:
                return "Items/Portal/" + idNumber;
            case 5:
                return "Items/Statue/Statue";
            default:
                Debug.LogWarning ("except value: " + preId);
                return null;
        }
    }
}