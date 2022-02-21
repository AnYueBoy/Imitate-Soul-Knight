/*
 * @Author: l hy 
 * @Date: 2021-12-27 13:30:56 
 * @Description: 四向攻击
 */
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class FourAttack : ActionNode {
    private BaseEnemy agentInstance;
    protected override void OnEnter () {
        agentInstance = (BaseEnemy) agent;
        this.attackHandler ();
    }

    protected override RunningStatus OnExecute () {
        return base.OnExecute ();
    }

    protected override void OnExit () {
        base.OnExit ();
    }

    private void attackHandler () {
        float attackOffset = agentInstance.attackOffset;

        Vector3 leftPos = this.agentInstance.transform.position + Vector3.left * attackOffset;
        Vector3 rightPos = this.agentInstance.transform.position + Vector3.right * attackOffset;
        Vector3 upPos = this.agentInstance.transform.position + Vector3.up * attackOffset;
        Vector3 downPos = this.agentInstance.transform.position + Vector3.down * attackOffset;

        string bulletLayer = agentInstance.bulletLayer;
        string bulletUrl = agentInstance.enemyConfigData.bulletUrl;
        float bulletSpeed = agentInstance.enemyConfigData.bulletSpeed;
        float bulletDamage = agentInstance.enemyConfigData.damage;

        // 左
        Vector3 leftEulerAngles = Util.GetWorldEulerAngles (this.agentInstance.transform, Vector3.zero);
        ModuleManager.instance.bulletManager.spawnBullet (leftPos, leftEulerAngles, -1, bulletLayer, bulletUrl, bulletSpeed, bulletDamage);

        // 右
        Vector3 rightEulerAngles = Util.GetWorldEulerAngles (this.agentInstance.transform, Vector3.zero);
        ModuleManager.instance.bulletManager.spawnBullet (rightPos, rightEulerAngles, 1, bulletLayer, bulletUrl, bulletSpeed, bulletDamage);

        // 上
        Vector3 upEulerAngles = Util.GetWorldEulerAngles (this.agentInstance.transform, new Vector3 (0, 0, 90));
        ModuleManager.instance.bulletManager.spawnBullet (upPos, upEulerAngles, 1, bulletLayer, bulletUrl, bulletSpeed, bulletDamage);

        // 下
        Vector3 downEulerAngles = Util.GetWorldEulerAngles (this.agentInstance.transform, new Vector3 (0, 0, -90));
        ModuleManager.instance.bulletManager.spawnBullet (downPos, downEulerAngles, 1, bulletLayer, bulletUrl, bulletSpeed, bulletDamage);
    }

}