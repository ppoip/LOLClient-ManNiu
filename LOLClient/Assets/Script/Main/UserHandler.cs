using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using GameProtocal.dto;

public class UserHandler : MonoSingleton<UserHandler>, IHandler
{
    /// <summary> 显示创建召唤师面板委托 </summary>
    public Action OnShowCreatePanel;

    /// <summary> 隐藏创建召唤师面板委托 </summary>
    public Action OnHideCreatePanel;

    /// <summary> 隐藏触控Mask委托 </summary>
    public Action OnHideMask;

    private void Awake()
    {
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_USER, this);
    }

    public void OnMessageReceived(SocketModel model)
    {
        switch (model.command)
        {
            case UserProtocal.CREATE_SRES:
                ProcessCreate(model.GetMessage<bool>());
                break;

            case UserProtocal.GET_INFO_SRES:
                ProcessGetInfo(model.GetMessage<UserDTO>());
                break;

            case UserProtocal.ONLINE_SRES:
                ProcessOnline(model.GetMessage<bool>());
                break;
        }
    }

    /// <summary>
    /// 处理创建角色
    /// </summary>
    /// <param name="isSuccessful"></param>
    private void ProcessCreate(bool isSuccessful)
    {
        if (isSuccessful)
        {
            //创建角色成功
            Debug.Log("创建角色成功");
            //关闭创角面板
            if (OnHideCreatePanel != null)
                OnHideCreatePanel();
            //发起获取角色信息请求
            NetIO.Instance.Write(Protocal.TYPE_USER, 0, UserProtocal.GET_INFO_CREQ, 0);
        }
        else
        {
            //创建角色失败
            Debug.Log("创建角色失败");
        }
    }

    /// <summary>
    /// 处理获取角色信息
    /// </summary>
    /// <param name="dto"></param>
    private void ProcessGetInfo(UserDTO dto)
    {
        if (dto.name == null || dto.name == string.Empty)
        {
            //账号下没有角色
            Debug.Log("该账号没有角色");
            //显示创建角色面板
            if (OnShowCreatePanel != null)
                OnShowCreatePanel();
        }
        else
        {
            //有角色
            //保存角色信息
            GameDataModelManager.Instance.GetModel<MainDataModel>().UserDTO = dto;

            //请求登陆角色
            NetIO.Instance.Write(Protocal.TYPE_USER, 0, UserProtocal.ONLINE_CREQ, 0);
        }
    }
    
    /// <summary>
    /// 处理登陆角色
    /// </summary>
    /// <param name="isSuccessful"></param>
    private void ProcessOnline(bool isSuccessful)
    {
        if (isSuccessful)
        {
            Debug.Log("角色上线成功");
            //隐藏Mask，用户可点击ui
            if (OnHideMask != null)
                OnHideMask();
        }
        else
        {
            Debug.Log("角色已经在线");
            //隐藏Mask，用户可点击ui
            if (OnHideMask != null)
                OnHideMask();
        }
    }

}
