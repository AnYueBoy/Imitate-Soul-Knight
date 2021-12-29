/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:25:31 
 * @Description: 野猪怪
 */

using System.Collections.Generic;
using DG.Tweening;
using UFramework;
using UFramework.AI.BehaviourTree;
using UFramework.FrameUtil;
using UnityEngine;

public class YeZhu : BaseEnemy {

    [SerializeField]
    private GameObject sprintEffect;

    private void Start () {
        blackboardMemory = new BlackBoardMemory ();
        BTNode = new ParallelNode (1).addChild (
            new SelectorNode ().addChild (
                new YeZhuDeadAction (),
                new SequenceNode ().addChild (
                    new YeZhuIdleAction (),
                    new YeZhuProbingAction (),
                    new YeZhuAttackAction ())
            )
        );
    }

    #region 攻击状态
    public void getAimToPlayerPath () {
        Vector3 playerPos = ModuleManager.instance.playerManager.getPlayerTrans ().position;
        this.pathPosList = this._pathFinding.findPath (this.transform.position, playerPos);
        if (this.pathPosList == null) {
            return;
        }
        this.drawPathList = new List<Vector3> (this.pathPosList);
        this.curMoveSpeed = this.enemyConfigData.sprintSpeed;
        this.curMoveIndex = 0;
        this.getNextTargetPos ();
    }

    public void showAttackEffect () {
        this.sprintEffect.SetActive (true);
    }

    public void hideAttackEffect () {
        this.sprintEffect.SetActive (false);
    }

    public float getDamageValue () {
        return this.enemyConfigData.damage;
    }
    #endregion

    #region  数据字段
    private float probingTimer = 0;
    private readonly float _probingInterval = 4;
    private readonly int _probingMinDistance = 2;
    private readonly int _probingMaxDistance = 4;
    #endregion

    #region  数据访问

    public float probingInterval {
        get {
            return this._probingInterval;
        }
    }

    public int probingMinDistance {
        get {
            return this._probingMinDistance;
        }
    }

    public int probingMaxDistance {
        get {
            return this._probingMaxDistance;
        }
    }
    #endregion
}