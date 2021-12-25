/*
 * @Author: l hy 
 * @Date: 2021-11-25 13:43:14 
 * @Description: 野猪攻击状态
 */

using UFramework;
using UFramework.AI.BehaviourTree;
using UnityEngine;

public class YeZhuAttackAction : ActionNode {
    protected override void onEnter () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.playMoveAni ();
        yeZhu.getAimToPlayerPath ();
        yeZhu.showAttackEffect ();
    }

    protected override RunningStatus onExecute () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.moveToTargetPos ();
        if (yeZhu.aimToPlayerDistance () < 0.45f) {
            ModuleManager.instance.playerManager.injured (yeZhu.getDamageValue ());
        }
        if (yeZhu.isReachEnd ()) {
            return RunningStatus.Failed;
        }
        return RunningStatus.Executing;
    }

    protected override void onExit () {
        YeZhu yeZhu = (YeZhu) agent;
        yeZhu.hideAttackEffect ();
        yeZhu.resetAttackState ();
    }
}