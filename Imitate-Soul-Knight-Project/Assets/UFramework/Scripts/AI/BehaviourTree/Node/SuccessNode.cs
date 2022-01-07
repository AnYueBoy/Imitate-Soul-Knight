/*
 * @Author: l hy 
 * @Date: 2021-12-29 09:39:16 
 * @Description: 成功节点
 */
using UFramework.AI.BehaviourTree;
public class SuccessNode : DecoratorNode {
    public SuccessNode (BaseNode child) : base (child) { }

    protected override RunningStatus onUpdate () {
        BaseNode childNode = m_Children[0];
        RunningStatus runningStatus = childNode.update (agent, blackBoardMemory, dt);
        if (runningStatus != RunningStatus.Executing) {
            this.curNodeRunningStatus = RunningStatus.Success;
            return RunningStatus.Success;
        } else {
            this.curNodeRunningStatus = RunningStatus.Executing;
            return RunningStatus.Executing;
        }
    }

    protected override void onReset () {
        m_Children[0].reset ();
    }
}