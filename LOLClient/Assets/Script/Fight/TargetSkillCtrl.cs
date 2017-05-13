using GameProtocal;
using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkillCtrl : MonoBehaviour
{
    private int damageSrcId;
    private int damageTargetId;
    private int skillId;
    private Transform targetTransform;
    private MainDataModel mainDataModel;


    private void Awake()
    {
        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();
    }

    public void Init(int damageSrcId,int damageTargetId,int skillId, Transform targetTransform)
    {
        this.damageSrcId = damageSrcId;
        this.damageTargetId = damageTargetId;
        this.skillId = skillId;
        this.targetTransform = targetTransform;
    }

    private void Update()
    {
        //计算与目标的距离
        float distance = Vector3.Distance(this.transform.position, this.targetTransform.position + new Vector3(0, 2, 0));

        if (distance <= 1)
        {
            //发送伤害到目标请求
            DamageDTO dto = new DamageDTO()
            {
                damageSrcId = this.damageSrcId,
                skillId = this.skillId,
                parameters = new int[1][] { new int[1] { damageTargetId } }
            };
            //只有发起伤害者是自己控制的角色才可发起伤害请求
            if (mainDataModel.UserDTO.id == damageSrcId)
            {
                NetIO.Instance.Write(Protocal.TYPE_FIGHT, 0, FightProtocal.DAMAGE_CREQ, dto);
            }

            //销毁自身
            Destroy(this.gameObject);
        }
        else
        {
            //方向
            Vector3 direction = (targetTransform.position + new Vector3(0, 2, 0) - this.transform.position).normalized;
            this.transform.Translate(direction * Time.deltaTime * 12);
        }


    }



}
