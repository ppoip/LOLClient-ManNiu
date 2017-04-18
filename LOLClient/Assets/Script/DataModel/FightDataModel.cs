using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDataModel : AbsGameDataModel
{
    private FightRoomModels _fightRoomModels;
    public  FightRoomModels FightRoomModels
    {
        get
        {
            return _fightRoomModels;
        }
        set
        {
            var oldValue = _fightRoomModels;
            _fightRoomModels = value;
            BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.FightRoomModels, oldValue = oldValue, newValue = value });
        }
    }





    public enum ModelValueType
    {
        FightRoomModels         //房间所有战斗模型
    }
}
