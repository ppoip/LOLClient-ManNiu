using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using GameProtocal.dto.fight;

public class FightHandler : MonoSingleton<FightHandler>, IHandler
{
    private void Awake()
    {
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_FIGHT, this);
    }

    public void OnMessageReceived(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocal.START_BRO:
                ProcessStart(model.message as FightRoomModels);
                break;
        }
    }

    /// <summary>
    /// 处理游戏开始
    /// </summary>
    /// <param name="models"></param>
    private void ProcessStart(FightRoomModels models)
    {

    }

}
