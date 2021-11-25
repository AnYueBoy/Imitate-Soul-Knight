using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:25:31 
 * @Description: 野猪怪
 */

using UFramework;
using UFramework.AI.BehaviourTree.Node;
using UFramework.AI.BlackBoard;
using UFramework.FrameUtil;
using UnityEngine;

public class YeZhu : BaseEnemy {
    private void Start () {
        blackboardMemory = new BlackBoardMemory ();
        BTNode = new ParallelNode (1).addChild (
            new SelectorNode ().addChild (
                new YeZhuDeadAction (),
                new YeZhuIdleAction (),
                new YeZhuProbingAction (),
                new YeZhuAttackAction ()
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
    private readonly float probingMinDistance = 3;
    private readonly float probingMaxDistance = 6;

    public void executeProbing () {
        float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
        this.probingTimer += dt;
    }

    public bool canExecuteProbing () {
        return this.probingTimer < this.probingInterval;
    }

    public void resetProbingState () {
        this.pathPosList = null;
        this.probingTimer = 0;
    }

    private Cell getTargetCell () {
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        float randomProbingDistance = CommonUtil.getRandomValue (this.probingMinDistance, this.probingMaxDistance);

        Vector3 leftPos = playerPos + Vector3.left * randomProbingDistance;
        Vector3 rightPos = playerPos + Vector3.right * randomProbingDistance;
        Vector3 upPos = playerPos + Vector3.up * randomProbingDistance;
        Vector3 downPos = playerPos + Vector3.down * randomProbingDistance;

        List<Vector3> posList = new List<Vector3> () { leftPos, rightPos, upPos, downPos };

        foreach (var pos in posList) {
            Cell targetCell = this.pathFinding.getGridByPos (pos);
            if (targetCell == null) {
                continue;
            }

            if (targetCell.isObstacle) {
                continue;
            }

            return targetCell;
        }
        return null;
    }

    public void generateProbingPath () {
        Cell targetCell = this.getTargetCell ();
        if (targetCell == null) {
            this.probingTimer = this.probingInterval;
            return;
        }

        this.pathPosList = this.pathFinding.findPath (this.transform.position, targetCell.worldPos);
        if (this.pathFinding == null) {
            this.probingTimer = this.probingInterval;
            return;
        }

        this.drawPathList = new List<Vector3> (this.pathPosList);
        this.curMoveIndex = 0;
        this.getNextTargetPos ();
    }

    private Vector3 targetPos = Vector3.zero;

    private Vector3 tempMoveDir = Vector3.zero;

    public int curMoveIndex = -1;

    public void moveToTargetPos () {
        if (this.pathPosList == null) {
            return;
        }

        float dt = this.blackboardMemory.getValue<float> ((int) BlackItemEnum.DT);
        // 步长
        float step = this.enemyConfigData.moveSpeed * dt;

        // 笛卡尔分量
        float horizontalStep = step * this.tempMoveDir.x;
        float verticalStep = step * this.tempMoveDir.y;

        Vector3 toWardsDir = (ModuleManager.instance.playerManager.getPlayerTrans ().position - this.transform.position).normalized;
        if (toWardsDir.x > 0) {
            this.transform.localScale = Vector3.one;
        } else {
            this.transform.localScale = new Vector3 (-1, 1, 1);
        }

        this.transform.position += new Vector3 (horizontalStep, verticalStep, 0);

        float distance = (this.transform.position - targetPos).magnitude;
        if (distance < step) {
            this.transform.position = targetPos;
            this.curMoveIndex++;
            this.getNextTargetPos ();
        }

        this.drawPath (Color.red);
    }

    private List<Vector3> drawPathList = new List<Vector3> ();

    private void drawPath (Color color) {
        if (this.drawPathList == null || this.drawPathList.Count <= 0) {
            return;
        }

        if ((this.transform.position - this.drawPathList[0]).magnitude <= 0.4f) {
            this.drawPathList.RemoveAt (0);
        }
        CommonUtil.drawPath (this.drawPathList, color);
        if (this.drawPathList.Count > 0) {
            CommonUtil.drawLine (this.transform.position, this.drawPathList[0], color);
        }
    }

    private List<Vector3> pathPosList = null;

    private void getNextTargetPos () {
        if (this.curMoveIndex >= this.pathPosList.Count) {
            return;
        }
        this.targetPos = this.pathPosList[this.curMoveIndex];
        this.tempMoveDir = (this.targetPos - this.transform.position).normalized;
    }

    public bool isReachEnd () {
        return this.pathPosList == null || this.curMoveIndex >= this.pathPosList.Count;
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