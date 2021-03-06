﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using GameProtocal.dto.fight;

public class FightHandler : MonoSingleton<FightHandler>, IHandler
{
    private FightDataModel fightDataModel;

    private void Awake()
    {
        fightDataModel = GameDataModelManager.Instance.GetModel<FightDataModel>();
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_FIGHT, this);
    }

    public void OnMessageReceived(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocal.START_BRO:
                ProcessStart(model.message as FightRoomModels);
                break;

            case FightProtocal.MOVE_BRO:
                ProcessHeroMove(model.GetMessage<HeroMoveDto>());
                break;

            case FightProtocal.ATTACK_BRO:
                ProcessHeroAttack(model.message as HeroAttackDto);
                break;

            case FightProtocal.DAMAGE_BRO:
                ProcessDamage(model.message as DamageDTO);
                break;

            case FightProtocal.SKILL_UP_SRES:
                ProcessSkillUp(model.message as FightSkill);
                break;
        }
    }

    /// <summary>
    /// 处理游戏开始
    /// </summary>
    /// <param name="models"></param>
    private void ProcessStart(FightRoomModels models)
    {
        fightDataModel.FightRoomModels = models;
    }

    /// <summary>
    /// 处理英雄移动
    /// </summary>
    /// <param name="dto"></param>
    private void ProcessHeroMove(HeroMoveDto dto)
    {
        fightDataModel.BroadcastHeroMove(dto);
    }

    /// <summary>
    /// 处理英雄普通攻击
    /// </summary>
    /// <param name="dto"></param>
    private void ProcessHeroAttack(HeroAttackDto dto)
    {
        fightDataModel.BroadcastHeroAttack(dto);
    }

    /// <summary>
    /// 处理伤害
    /// </summary>
    /// <param name="dto"></param>
    private void ProcessDamage(DamageDTO dto)
    {
        fightDataModel.BroadcastDamage(dto);
    }

    private void ProcessSkillUp(FightSkill skillDTO)
    {
        fightDataModel.NoticeSkillUp(skillDTO);
    }

    /// <summary>
    /// 发送移动请求给服务器
    /// </summary>
    /// <param name="dto"></param>
    public void SendMoveRequest(HeroMoveDto dto)
    {
        NetIO.Instance.Write(Protocal.TYPE_FIGHT, 0, FightProtocal.MOVE_CREQ, dto);
    }

}
