using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : MonoBehaviour
{
    private Image iconImage;
    private Image maskImage;
    private Button skillButton;

    public FightSkill FightSkill { get; set; }

    private void Awake()
    {
        iconImage = GetComponent<Image>();
        skillButton = GetComponent<Button>();
        maskImage = transform.Find("ColdMask").GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Init(FightSkill data)
    {
        FightSkill = data;
        maskImage.fillAmount = 0;  //默认不冷却
        skillButton.interactable = false; //一开始0级不可点击

        RefreshUI(data);
    }

    /// <summary>
    /// 刷新UI显示
    /// </summary>
    /// <param name="data"></param>
    public void RefreshUI(FightSkill data)
    {
        iconImage.sprite = Resources.Load<Sprite>("SkillIcon/" + data.code.ToString());
        maskImage.sprite = Resources.Load<Sprite>("SkillIcon/" + data.code.ToString());
    }

    /// <summary>
    /// 当自身被点击
    /// </summary>
    private void OnClick()
    {

    }

    /// <summary>
    /// 当该技能的升级按钮被点击时
    /// </summary>
    public void OnPlusBtnClick()
    {

    }

}
