/*
 * @Author: l hy 
 * @Date: 2021-10-28 17:25:31 
 * @Description: 野猪怪
 */

using System;
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
        BTNode = new ParallelNode (1).AddChild (
            new SelectorNode ().AddChild (
                new SequenceNode ().AddChild (
                    new NormalDead ().SetPreCondition (new IsDead ()),
                    new Rebound ()
                ),
                new SequenceNode ().AddChild (
                    new IdleAction (),
                    new SuccessNode (
                        new SequenceNode ().AddChild (
                            new GetRoundRandomPos (),
                            new MoveToTarget ()
                        )
                    ),
                    new FailureNode (
                        new ParallelNode (1).AddChild (
                            new YeZhuMeleeEffect (),
                            new SuccessNode (new SequenceNode ().AddChild (
                                new GetTargetPosition (),
                                new MoveToTarget ()
                            ))
                        )
                    )
                )
            )
        );
    }

    public override void init (EnemyConfigData enemyConfigData, Func<bool> isRoomActive) {
        base.init (enemyConfigData, isRoomActive);
        this._meleeAttackRange = 0.4f;
    }

    #region  数据字段
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

    public GameObject sprintEffectNode {
        get {
            return this.sprintEffect;
        }
    }
    #endregion
}