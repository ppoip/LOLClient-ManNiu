using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerFightModel data;

    private NavMeshAgent navAgent;
    private Animator animator;

    private Slider hpSlider;
    private TextMesh textName;

    /// <summary> 当前的英雄状态 </summary>
    private int curAnimState = AnimStateConst.IDLE;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        hpSlider = transform.Find("Canvas/HPBar").GetComponent<Slider>();
        textName = transform.Find("Canvas/HeroName").GetComponent<TextMesh>();
    }

    private void Update()
    {
        //调整UI
        CorrectHeroUI();

        if (curAnimState == AnimStateConst.RUN)
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

    public void SetData(PlayerFightModel model)
    {
        this.data = model;
        RefreshHeroUI();
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


    /// <summary>
    /// 使英雄UI面向相机
    /// </summary>
    private void CorrectHeroUI()
    {
        hpSlider.transform.rotation = Camera.main.transform.rotation;
        textName.transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// 刷新显示英雄的3DUI
    /// </summary>
    private void RefreshHeroUI()
    {
        Slider hpSlider = transform.Find("Canvas/HPBar").GetComponent<Slider>();
        TextMesh textName = transform.Find("Canvas/HeroName").GetComponent<TextMesh>();
        hpSlider.value = this.data.curHp / this.data.maxHp;
        textName.text = this.data.name;
    }

}
