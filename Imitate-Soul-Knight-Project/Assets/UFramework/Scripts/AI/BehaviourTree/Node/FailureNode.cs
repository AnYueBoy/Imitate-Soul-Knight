/*
 * @Author: l hy 
 * @Date: 2021-12-29 09:49:52 
 * @Description: 失败节点
 */
using UFramework.AI.BehaviourTree;
public class FailureNode : DecoratorNode {
    public FailureNode (BaseNode child) : base (child) { }

    protected override RunningStatus onUpdate () {
        BaseNode childNode = m_Children[0];
        RunningStatus runningStatus = childNode.update (agent, blackBoardMemory, dt);
        if (runningStatus != RunningStatus.Executing) {
            return RunningStatus.Failed;
        } else {
            return RunningStatus.Executing;
        }
    }

    protected override void onReset () {
        m_Children[0].reset ();
    }
}