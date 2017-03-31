using GameProtocal.dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDataModel : AbsGameDataModel
{
    /// <summary> 角色数据 </summary>
    private UserDTO _userDTO = null;
    public UserDTO UserDTO
    {
        set
        {
            UserDTO temp_old_value = _userDTO;
            _userDTO = value;
            BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.UserDTO, oldValue = temp_old_value, newValue = _userDTO });
        }
    }

    /// <summary> 房间内所有玩家model </summary>
    private SelectRoomDTO _selectRoomDTO=null;
    public SelectRoomDTO SelectRoomDTO
    {
        set
        {
            SelectRoomDTO oldValue = _selectRoomDTO;
            _selectRoomDTO = value;
            BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.SelectRoomModelArray, oldValue = oldValue, newValue = _selectRoomDTO });
        }

        get
        {
            return _selectRoomDTO;
        }
    }

    /// <summary> 修改房间某个model </summary>
    public SelectModel SetSelectModel
    {
        set
        {
            SelectModel oldValue,newValue;
            oldValue = new SelectModel();
            //get old value
            var tempValue = _selectRoomDTO.GetModelByUserId(value.userId);
            oldValue.userId = tempValue.userId;
            oldValue.name = tempValue.name;
            oldValue.isReady = tempValue.isReady;
            oldValue.isEnter = tempValue.isEnter;
            oldValue.hero = tempValue.hero;

            //new value
            newValue = _selectRoomDTO.SetModel(value);

            BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.SelectRoomModel, oldValue = oldValue, newValue = newValue });
        }
    }

    /// <summary> 房间内所有聊天信息 </summary>
    private List<string> _chatMessages = new List<string>();
    public string RoomChat
    {
        set
        {
            _chatMessages.Add(value);
            BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.Chat, oldValue = null, newValue = _chatMessages });
        }   
    }

    public void CleanRoomData()
    {
        //清楚所有与房间有关的数据
        _selectRoomDTO = null;
        _chatMessages.Clear();
    }


    public enum ModelValueType
    {
        UserDTO,               //角色信息
        SelectRoomModelArray,  //房间所有成员model
        SelectRoomModel,       //房间成员model
        Chat                   //聊天信息
    }
}
