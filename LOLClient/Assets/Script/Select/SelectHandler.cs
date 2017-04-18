using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using GameProtocal;
using GameProtocal.dto;
using UnityEngine.SceneManagement;

public class SelectHandler : MonoSingleton<SelectHandler>, IHandler
{
    /// <summary> 数据model </summary>
    private MainDataModel mainDataModel;

    /// <summary> 销毁房间委托 </summary>
    public Action OnRoomDestroy;

    private void Awake()
    {
        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>(); 
        GetComponent<NetMessageUtil>().handlers.Add(Protocal.TYPE_SELECT, this);
    }

    public void OnMessageReceived(SocketModel model)
    {
        switch (model.command)
        {
            //进入房间响应，获取到了房间内所有成员的数据
            case SelectProtocal.ENTER_SRES:
                ProcessEnterSres(model.GetMessage<SelectRoomDTO>());
                break;

            //有一个玩家进入了房间
            case SelectProtocal.ENTER_BRO:
                ProcessEnterBro(model.GetMessage<int>());
                break;

            //选择英雄响应
            case SelectProtocal.SELECT_SRES:
                ProcessSelectSres();
                break;

            //有一个玩家选择了某个英雄
            case SelectProtocal.SELECT_BRO:
                ProcessSelectBro(model.GetMessage<SelectModel>());
                break;

            //有一个玩家准备了
            case SelectProtocal.READY_BRO:
                ProcessReadyBro(model.GetMessage<SelectModel>());
                break;
            
            //有一个玩家发送了一条聊天信息
            case SelectProtocal.TALK_BRO:
                ProcessTalkBro(model.GetMessage<string>());
                break;
            
            //所有玩家均已准备，进入战斗
            case SelectProtocal.FIGHT_BRO:
                ProcessFightBro();
                break;
            
            //销毁房间
            case SelectProtocal.DESTROY_BRO:
                ProcessDestroyBro();
                break;
        }
    }

    private void ProcessEnterSres(SelectRoomDTO roomModelDto)
    {
        //保存房间所有玩家model
        mainDataModel.SelectRoomDTO = roomModelDto;
    }

    private void ProcessEnterBro(int userId)
    {
        //判断房间model是否还未初始化
        if (mainDataModel.SelectRoomDTO == null)
            return;

        var model = mainDataModel.SelectRoomDTO.GetModelByUserId(userId);
        //设置该玩家已准备
        model.isEnter = true;
        //通知
        mainDataModel.SetSelectModel = model;
    }

    private void ProcessSelectSres()
    {
        //选择英雄失败
    }

    private void ProcessSelectBro(SelectModel selectModel)
    {
        //判断房间model是否还未初始化
        if (mainDataModel.SelectRoomDTO == null)
            return;

        //通知
        mainDataModel.SetSelectModel = selectModel;
    }

    private void ProcessReadyBro(SelectModel selectModel)
    {
        //判断房间model是否还未初始化
        if (mainDataModel.SelectRoomDTO == null)
            return;

        //通知
        mainDataModel.SetSelectModel = selectModel;
    }

    private void ProcessTalkBro(string message)
    {
        //添加聊天信息
        mainDataModel.RoomChat = message;
    }

    private void ProcessFightBro()
    {
        //直接加载战斗场景
        SceneManager.LoadScene("fight");
    }

    private void ProcessDestroyBro()
    {
        //销毁房间委托
        if (OnRoomDestroy != null)
            OnRoomDestroy();
    }
}
