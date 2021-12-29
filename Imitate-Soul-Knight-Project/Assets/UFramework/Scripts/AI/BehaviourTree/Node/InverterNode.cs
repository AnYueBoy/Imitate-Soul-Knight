/*
 * @Author: l hy 
 * @Date: 2021-12-29 09:32:08 
 * @Description: 逆变节点
 */

using UFramework.AI.BehaviourTree;
public class InverterNode : DecoratorNode {
    public InverterNode (BaseNode child) : base (child) { }

    protected override RunningStatus onUpdate () {
        BaseNode childNode = m_Children[0];
        RunningStatus runningStatus = childNode.update (agent, blackBoardMemory, dt);
        if (runningStatus == RunningStatus.Failed) {
            return RunningStatus.Success;
        }

        if (runningStatus == RunningStatus.Success) {
            return RunningStatus.Failed;
        }
        return runningStatus;
    }

    protected override void onReset () {
        m_Children[0].reset ();
    }
}