using GameProtocal.dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFrame : MonoBehaviour
{
    private Image playerFrameImage;
    private Text playerNameText;
    private Image playerHeroIconImage;

    private void Awake()
    {
        playerFrameImage = GetComponent<Image>();
        playerNameText = transform.Find("PlayerName").GetComponent<Text>();
        playerHeroIconImage = transform.Find("HeroHead/Image").GetComponent<Image>();
    }

    /// <summary>
    /// 将model数据刷新到ui上
    /// </summary>
    /// <param name="model"></param>
    public void RefreshUI(SelectModel model)
    {
        //hero icon
        if (model.isEnter)
        {
            if (model.hero != -1)
            {
                playerHeroIconImage.sprite = Resources.Load<Sprite>("HeroIcon/" + model.hero.ToString());
            }
            else
            {
                playerHeroIconImage.sprite = Resources.Load<Sprite>("HeroIcon/Grid");
            }
        }
        else
        {
            playerHeroIconImage.sprite = Resources.Load<Sprite>("HeroIcon/Image 19");
        }

        //name
        playerNameText.text = model.name;

        //frame color
        if (model.isReady)
        {
            playerFrameImage.color = Color.red;
        }
        else
        {
            playerFrameImage.color = Color.white;
        }
    }

}
