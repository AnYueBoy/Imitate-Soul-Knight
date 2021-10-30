using System;
using System.Collections;
using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UFramework.AI.BehaviourTree.Agent;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UnityEngine;

public class IdleAction : ActionNode {

    protected override bool onEvaluate (IAgent agent, BlackBoardMemory workingMemory) {
        // 玩家未进入此房间时，角色处于待机状态
        return true;
    }

    protected override void onEnter (IAgent agent, BlackBoardMemory blackBoardMemory) {
        LanJingGuai lanJingGuai = (LanJingGuai) agent;
        lanJingGuai.playIdleAni ();
    }

    protected override RunningStatus onExecute (IAgent agent, BlackBoardMemory workingMemory) {

        return RunningStatus.Finished;
    }

}