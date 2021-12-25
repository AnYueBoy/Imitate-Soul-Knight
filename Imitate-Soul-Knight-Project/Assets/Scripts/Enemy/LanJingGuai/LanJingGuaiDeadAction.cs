/*
 * @Author: l hy 
 * @Date: 2021-11-16 08:24:33 
 * @Description: 蓝精怪死亡状态
 */
using UFramework.AI.BehaviourTree;
using UnityEngine;

public class LanJingGuaiDeadAction : ActionNode {
    protected override bool onEvaluate () {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        return lanJingGuai.isDead ();
    }

    protected override void onEnter () {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playerDeadAni ();
        lanJingGuai.invalidCollider ();

        Vector2 bulletDir = lanJingGuai.getDamageBulletDir ();
        lanJingGuai.deadMove (bulletDir);
    }

    protected override RunningStatus onExecute () {
        return RunningStatus.Executing;
    }
}