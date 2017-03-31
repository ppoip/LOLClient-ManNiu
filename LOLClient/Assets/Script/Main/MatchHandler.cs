using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using UnityEngine.SceneManagement;

public class MatchHandler : MonoSingleton<MatchHandler>, IHandler
{
    private void Awake()
    {
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_MATCH, this);
    }

    public void OnMessageReceived(SocketModel model)
    {
        switch (model.command)
        {
            case MatchProtocal.ENTER_SELECT_BRO:
                ProcessEnterSelectRoom();
                break;
        }

    }

    private void ProcessEnterSelectRoom()
    {
        //直接进入选人房间场景
        SceneManager.LoadScene("select");
    }
}
