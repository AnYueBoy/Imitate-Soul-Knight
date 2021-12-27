/*
 * @Author: l hy 
 * @Date: 2021-01-16 15:40:08 
 * @Description: 队列节点（控制流节点）
 */

namespace UFramework.AI.BehaviourTree {
    public class SequenceNode : BaseNode {

        private int m_currentNodeIndex = -1;

        protected override RunningStatus onUpdate () {
            if (m_Children.Count == 0) {
                return RunningStatus.Success;
            }

            if (m_currentNodeIndex < 0) {
                m_currentNodeIndex = 0;
            }

            for (int i = m_currentNodeIndex; i < m_Children.Count; ++i) {
                RunningStatus status = m_Children[i].update (this.agent, this.blackBoardMemory, dt);
                if (status != RunningStatus.Success) {
                    return status;
                }

                m_currentNodeIndex++;
            }

            return RunningStatus.Success;
        }

        protected override void onReset () {
            foreach (BaseNode node in m_Children) {
                node.reset ();
            }

            m_currentNodeIndex = -1;
        }
    }
}