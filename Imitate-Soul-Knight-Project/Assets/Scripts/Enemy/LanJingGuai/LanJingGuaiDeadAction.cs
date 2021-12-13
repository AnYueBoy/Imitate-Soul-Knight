/*
 * @Author: l hy 
 * @Date: 2021-11-16 08:24:33 
 * @Description: 蓝精怪死亡状态
 */
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UnityEngine;

public class LanJingGuaiDeadAction : ActionNode {
    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        return lanJingGuai.isDead ();
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playerDeadAni ();
        lanJingGuai.invalidCollider ();

        Vector2 bulletDir = lanJingGuai.getDamageBulletDir ();
        lanJingGuai.deadMove (bulletDir);
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {
        return RunningStatus.Executing;
    }
}