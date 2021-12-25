/*
 * @Author: l hy 
 * @Date: 2021-11-25 08:24:35 
 * @Description: 野猪死亡状态
 */

using UFramework;
using UFramework.AI.BehaviourTree;
using UnityEngine;

public class YeZhuDeadAction : ActionNode {
    protected override bool onEvaluate () {
        YeZhu yeZhu = (YeZhu) agent;
        return yeZhu.isDead ();
    }

    protected override void onEnter () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playerDeadAni ();
        yeZhu.invalidCollider ();

        Vector2 bulletDir = yeZhu.getDamageBulletDir ();
        yeZhu.deadMove (bulletDir);
    }

    protected override RunningStatus onExecute () {
        return RunningStatus.Executing;
    }
}