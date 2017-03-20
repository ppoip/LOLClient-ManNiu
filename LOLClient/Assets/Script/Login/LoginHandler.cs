using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using UnityEngine.SceneManagement;

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
        switch (model.command)
        {
            case LoginProtocal.LOGIN_SRES:
                //登陆响应
                LoginProcess(model.GetMessage<int>());
                break;

            case LoginProtocal.REG_SRES:
                //注册响应
                regProcess(model.GetMessage<int>());
                break;
        }
    }

    private void LoginProcess(int retCode)
    {
        switch (retCode)
        {
            case 0:
                //登陆成功
                //跳转场景
                SceneManager.LoadScene("main");
            break;

            case -1:
                //账号不存在
                LoginPanel.Instance.ShowMessage("账号不存在");
            break;

            case -2:
                //账号已在线
                LoginPanel.Instance.ShowMessage("账号已在线");
                break;

            case -3:
                //密码错误
                LoginPanel.Instance.ShowMessage("密码错误");
                break;

            case -4:
                //输入不合法
                LoginPanel.Instance.ShowMessage("输入不合法");
                break;
        }
    }

    private void regProcess(int retCode)
    {
        switch (retCode)
        {
            case 0:
                //注册成功
                LoginPanel.Instance.ShowMessage("注册成功");
                break;

            case 1:
                //账号重复
                LoginPanel.Instance.ShowMessage("账号重复");
                break;

            case 2:
                //账号不合法
                LoginPanel.Instance.ShowMessage("账号不合法");
                break;
        }
    }

}
