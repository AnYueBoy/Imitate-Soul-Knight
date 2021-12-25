/*
 * @Author: l hy 
 * @Date: 2021-01-16 15:45:39 
 * @Description: 行为树执行器
 */

using UnityEngine;

namespace UFramework.AI.BehaviourTree {
    public class BehaviourTreeRunner : MonoBehaviour {

        public void execute (BaseNode root, IAgent agent, BlackBoardMemory workingMemory) {
            RunningStatus status = root.update (agent, workingMemory);
            if (status != RunningStatus.Executing) {
                root.reset ();
            }
        }
    }
}