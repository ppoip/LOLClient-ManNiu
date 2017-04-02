using GameProtocal.dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroListFrame : MonoBehaviour
{
    private GameObject grid;

    /// <summary> 数据model </summary>
    private MainDataModel mainDataModel;

    private void Awake()
    {
        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();
        grid = transform.Find("Mask/Content").gameObject;
    }

    /// <summary>
    /// 刷新显示英雄列表UI
    /// </summary>
    /// <param name="heroIds"></param>
    public void RefreshHeroList(List<int> heroIds)
    {
        //Clear
        foreach(Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(int heroId in heroIds)
        {
            //实例化
            var heroFrame = GameObject.Instantiate(Resources.Load<GameObject>("UIPrefabs/HeroFrame")).GetComponent<HeroFrame>();
            heroFrame.transform.parent = grid.transform;
            heroFrame.RefreshSelf(heroId);
            heroFrame.transform.localScale = new Vector3(1, 1, 1);

            //获取对应的队伍
            List<SelectModel> ourPart = null;
            List<SelectModel> enemyPart = null;
            SelectRoomDTO dto = mainDataModel.SelectRoomDTO;
            if (new List<SelectModel>(dto.teamOne).Find(x => x.userId == mainDataModel.UserDTO.id) != null)
            {
                ourPart = new List<SelectModel>(dto.teamOne);
                enemyPart = new List<SelectModel>(dto.teamTwo);
            }
            if (new List<SelectModel>(dto.teamTwo).Find(x => x.userId == mainDataModel.UserDTO.id) != null)
            {
                ourPart = new List<SelectModel>(dto.teamTwo);
                enemyPart = new List<SelectModel>(dto.teamOne);
            }

            //我方选择的英雄集合
            List<int> ownPartSelectHero = new List<int>();
            foreach(var model in ourPart)
            {
                //过滤自己
                //if (model.userId != mainDataModel.UserDTO.id)
                //{
                    ownPartSelectHero.Add(model.hero);
                //}
            }

            //设置是否能被选择
            if (ownPartSelectHero.Contains(heroFrame.HeroId))
            {
                //不可选
                heroFrame.DeActive();
            }
            else
            {
                //可选
                heroFrame.Active();
            }
        }
    }
}
