using UFramework;

public class PassiveWeapon : PassiveTriggerItem {
    private void OnEnable () {
        this.triggerDistance = 0.85f;
    }

    public override void triggerHandler () {
        base.triggerHandler ();

        // 装备替换装备
        BaseWeapon weapon = this.GetComponent<BaseWeapon> ();
        ModuleManager.instance.playerManager.equipmentWeapon (weapon);
    }
}