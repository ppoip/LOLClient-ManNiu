﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 阿狸的控制类
/// </summary>
public class HeroAliCtrl : PlayerController
{
    //当前普通攻击目标
    private GameObject[] curTargets=null;

    public override void Attack(GameObject[] targets)
    {
        base.Attack(targets);
        if(curAnimState == AnimStateConst.RUN)
        {
            navAgent.Stop();
            navAgent.ResetPath();
            navAgent.CompleteOffMeshLink();
        }
        curTargets = targets;
        //面向敌人
        if (curTargets.Length != 0)
        {
            this.transform.LookAt(targets[0].transform);
        }
        //播放普通攻击动画
        SetState(AnimStateConst.ATTACK);
    }

    public override void AttackAnimEnd()
    {
        base.AttackAnimEnd();

        //实例化一个球特效飞向敌人
        //TODO

        //切换idle状态
        SetState(AnimStateConst.IDLE);
    }

}
