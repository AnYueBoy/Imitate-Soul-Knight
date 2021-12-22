/*
 * @Author: l hy 
 * @Date: 2021-10-22 18:24:05 
 * @Description: 武器基类
 */
using UFramework;
using UnityEngine;

[RequireComponent (typeof (PassiveWeapon))]
public abstract class BaseWeapon : MonoBehaviour {

    protected ItemIdEnum id;

    protected WeaponConfigData weaponConfigData;

    protected float attackInterval = 0;

    protected string weaponLayer;

    protected bool isInAttackState = false;

    [SerializeField] protected SpriteRenderer weaponSpriteRender;

    public virtual void init (ItemIdEnum id) {
        this.id = id;
        this.weaponConfigData = ModuleManager.instance.configManager.weaponConfig.getWeaponConfigDataById (this.id);

        this.attackInterval = weaponConfigData.attackInterval;
    }

    public virtual void equipment (string bulletLayer) {
        this.weaponLayer = bulletLayer;
    }

    public abstract void attack (float args);

    public abstract void localUpdate (float dt);

    public int getWeaponMpConsume () {
        return this.weaponConfigData.mpConsume;
    }

    public WeaponTypeEnum getWeaponType () {
        return (WeaponTypeEnum) this.weaponConfigData.weaponType;
    }

    public Sprite getWeaponSprite () {
        return this.weaponSpriteRender.sprite;
    }
}