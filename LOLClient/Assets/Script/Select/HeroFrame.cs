using GameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroFrame : MonoBehaviour
{
    private int _heroId = -1;
    public int HeroId { get { return _heroId; } }

    private Image heroIcon;
    private Button iconButton;

    private void Awake()
    {
        iconButton = GetComponent<Button>();
        iconButton.onClick.AddListener(OnHeroIconClick);
        heroIcon = GetComponent<Image>();
    }

    public void RefreshSelf(int heroId)
    {
        this._heroId = heroId;
        heroIcon.sprite = Resources.Load<Sprite>("HeroIcon/" + this._heroId.ToString());
    }

    private void OnHeroIconClick()
    {
        //Send 选择英雄请求
        NetIO.Instance.Write(Protocal.TYPE_SELECT, 0, SelectProtocal.SELECT_CREQ, this._heroId);
    }

    /// <summary>
    /// 能被点击
    /// </summary>
    public void Active()
    {
        iconButton.interactable = true;
    }

    /// <summary>
    /// 不能被点击
    /// </summary>
    public void DeActive()
    {
        iconButton.interactable = false;
    }

}
