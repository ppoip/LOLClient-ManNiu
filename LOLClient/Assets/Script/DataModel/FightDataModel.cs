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

    /// <summary>
    /// 广播英雄移动事件
    /// </summary>
    /// <param name="dto"></param>
    public void BroadcastHeroMove(HeroMoveDto dto)
    {
        BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.HeroMove, newValue = dto, oldValue = null });
    }

    /// <summary>
    /// 广播英雄普通攻击
    /// </summary>
    /// <param name="dto"></param>
    public void BroadcastHeroAttack(HeroAttackDto dto)
    {
        BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.HeroAttack, newValue = dto, oldValue = null });
    }

    /// <summary>
    /// 广播受到伤害
    /// </summary>
    /// <param name="dto"></param>
    public void BroadcastDamage(DamageDTO dto)
    {
        BroadcastEvent(new OnValueChangeArgs() { valueType = (int)ModelValueType.Damage, newValue = dto, oldValue = null });
    }


    public enum ModelValueType
    {
        FightRoomModels,         //房间所有战斗模型
        HeroMove,                //英雄移动
        HeroAttack,              //英雄普通攻击
        Damage
    }
}
