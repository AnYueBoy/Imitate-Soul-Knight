/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:25:31 
 * @Description: 野猪怪
 */

using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;

public class YeZhu : BaseEnemy {
    private void Start () {
        blackboardMemory = new BlackBoardMemory ();
        BTNode = new ParallelNode (1).addChild (
            new SelectorNode ().addChild (
                new YeZhuDeadAction (),
                new YeZhuIdleAction (),
                new YeZhuProbingAction ()
            )
        );
    }

    #region  动画状态
    public void playIdleAni () {
        this.animator.SetBool ("IsMove", false);
    }

    public void playMoveAni () {
        this.animator.SetBool ("IsMove", true);
    }

    public void playerDeadAni () {
        this.animator.SetBool ("IsDeath", true);
    }

    #endregion

    #region 试探攻击状态
    private float probingTimer = 0;
    private readonly float probingInterval = 4;

    public void executeProbing () {
        float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
        this.probingTimer += dt;
    }

    public bool canExecuteProbing () {
        return this.probingTimer < this.probingInterval;
    }

    public void resetProbingState () {
        this.probingTimer = 0;
    }

    #endregion

    #region 攻击状态

    #endregion

    #region 死亡状态 

    public void invalidCollider () {
        if (this.boxCollider2D.enabled) {
            this.boxCollider2D.enabled = false;
        }
    }

    #endregion
}