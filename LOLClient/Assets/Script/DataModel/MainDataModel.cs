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



    public enum ModelValueType
    {
        UserDTO
    }
}
