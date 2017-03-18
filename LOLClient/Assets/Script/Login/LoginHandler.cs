using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;

public class LoginHandler : MonoBehaviour,IHandler
{
    private void Awake()
    {
        //添加自身应用到NetMessageUtil.handlers
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_LOGIN, this);
    } 

    public void OnMessageReceived(SocketModel model)
    {
        Debug.Log("response type code : " + model.type.ToString() + "，command code : " + model.command.ToString() + "，message : " + model.message.ToString());
    }
}
