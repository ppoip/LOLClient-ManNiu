using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public FightDataModel data;

    private NavMeshAgent navAgent;
    private Animator animator;

    /// <summary> 当前的英雄状态 </summary>
    private int curAnimState = AnimStateConst.IDLE;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(curAnimState == AnimStateConst.RUN)
        {
            if (navAgent.remainingDistance <= 0 && navAgent.pathStatus == NavMeshPathStatus.PathComplete && !navAgent.pathPending)
            {
                SetState(AnimStateConst.IDLE);
            }
            else
            {
                if (navAgent.isOnOffMeshLink)
                {
                    navAgent.CompleteOffMeshLink();
                }
            }


        }
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="pos"></param>
    private void MoveTo(Vector3 pos)
    {
        navAgent.Stop();
        navAgent.ResetPath();
        navAgent.SetDestination(pos);
        SetState(AnimStateConst.RUN);
    }


    private void SetState(int state)
    {
        curAnimState = state;
        animator.SetInteger("state", state);
    }

}
