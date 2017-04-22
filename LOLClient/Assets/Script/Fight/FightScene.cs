﻿using GameProtocal;
using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScene : MonoBehaviour
{
    /// <summary> 数据Model </summary>
    private FightDataModel fightDataModel;

    /// <summary> 数据model </summary>
    private MainDataModel mainDataModel;

    [SerializeField]
    private Transform[] buildingPos;

    [SerializeField]
    private Transform[] heroStartPos;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Image headImage;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private UISkillItem[] uiSkillItems;

    [SerializeField]
    private CameraController mainCamera;

    public Dictionary<int, GameObject> teamOneObject = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> teamTwoObject = new Dictionary<int, GameObject>();

    [HideInInspector]
    public GameObject selfHero = null;


    private void Awake()
    {
        fightDataModel = GameDataModelManager.Instance.GetModel<FightDataModel>();
        fightDataModel.RegisterChangeEvent(OnFightModelChange);

        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();
    }

    private void Start()
    {
        //告诉服务器，加载完毕
        NetIO.Instance.Write(Protocal.TYPE_FIGHT, 0, FightProtocal.LOADING_COMPLETED_CQEQ, 0);
    }

    private void OnDestroy()
    {
        fightDataModel.RemoveChangeEvent(OnFightModelChange);
    }

    private void OnFightModelChange(AbsGameDataModel.OnValueChangeArgs args)
    {
        switch (args.valueType)
        {
            case (int)FightDataModel.ModelValueType.FightRoomModels:
                OnFightRoomModelsChange(args.newValue as FightRoomModels);
                break;
        }
    }

    /// <summary>
    /// 当获取到房间所有模型数据时触发
    /// </summary>
    /// <param name="models"></param>
    private void OnFightRoomModelsChange(FightRoomModels models)
    {
        //实例化英雄与建筑
        foreach (var item in models.teamOne)
        {
            GameObject go = null;
            if(item.modelType== ModelType.Building)
            {
                Transform pos = new List<Transform>(buildingPos).Find(x => x.gameObject.name == string.Format("1_{0}", item.code));
                go = GameObject.Instantiate<GameObject>
                    (Resources.Load<GameObject>(string.Format("FightPrefabs/Building/1_{0}", item.code)),
                    pos.transform.position, pos.transform.rotation);
            }else if(item.modelType == ModelType.Human)
            {
                Transform pos = new List<Transform>(heroStartPos).Find(x => x.gameObject.name == "HeroStartPos1");
                go = GameObject.Instantiate<GameObject>
                    (Resources.Load<GameObject>(string.Format("FightPrefabs/Player/{0}", item.code)),
                    pos.transform.position, pos.transform.rotation);
            }
            teamOneObject.Add(item.id, go);

            if (item.id == mainDataModel.UserDTO.id)
            {
                //刷新所有UI
                RefreshAllUI(item as PlayerFightModel);
                //标记自身英雄
                selfHero = go;
            }
        }
        foreach (var item in models.teamTwo)
        {
            GameObject go = null;
            if (item.modelType == ModelType.Building)
            {
                Transform pos = new List<Transform>(buildingPos).Find(x => x.gameObject.name == string.Format("2_{0}", item.code));
                go = GameObject.Instantiate<GameObject>
                    (Resources.Load<GameObject>(string.Format("FightPrefabs/Building/2_{0}", item.code)),
                    pos.transform.position, pos.transform.rotation);
            }
            else if (item.modelType == ModelType.Human)
            {
                Transform pos = new List<Transform>(heroStartPos).Find(x => x.gameObject.name == "HeroStartPos2");
                go = GameObject.Instantiate<GameObject>
                    (Resources.Load<GameObject>(string.Format("FightPrefabs/Player/{0}", item.code)),
                    pos.transform.position, pos.transform.rotation);
            }
            teamTwoObject.Add(item.id, go);

            if (item.id == mainDataModel.UserDTO.id)
            {
                //刷新所有UI
                RefreshAllUI(item as PlayerFightModel);
                //标记自身英雄
                selfHero = go;
            }
        }

    }

    /// <summary>
    /// 刷新显示所有UI
    /// </summary>
    /// <param name="model"></param>
    private void RefreshAllUI(PlayerFightModel model)
    {
        //user name
        nameText.text = model.name;

        //level
        levelText.text = model.level.ToString();

        //hp bar
        hpBar.value = model.curHp / (float)model.maxHp;

        //head icon
        headImage.sprite = Resources.Load<Sprite>("HeroIcon/" + model.code.ToString());

        //skill
        int i = 0;
        foreach (var item in model.skills)
        {
            uiSkillItems[i].Init(item);
            i++;
        }



    }

    private void Update()
    {
        if (selfHero)
        {
            //相机跟随自己的英雄
            mainCamera.LookAtTarget(selfHero.transform, new Vector3(-13.45f, 22.37f, -6.149994f));
        }
    }


}
