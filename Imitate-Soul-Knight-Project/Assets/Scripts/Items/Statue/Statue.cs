/*
 * @Author: l hy 
 * @Date: 2021-12-07 08:45:13 
 * @Description: 雕像
 */
using UFramework.GameCommon;
using UnityEngine;

public class Statue : PassiveTriggerItem {

    [SerializeField]
    private SpriteRenderer statue;

    private ItemIdEnum itemId;

    private void OnEnable () {
        this.triggerDistance = 2.3f;
    }

    public void init (ItemIdEnum itemId) {
        this.itemId = itemId;
        string url = "Statue/" + (int) itemId;
        this.statue.sprite = AssetsManager.instance.getAssetByUrlSync<Sprite> (url);
    }

    public override void triggerHandler () {
        base.triggerHandler ();
        Debug.Log ("trigger statue");
    }
}