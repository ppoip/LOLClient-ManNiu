using GameProtocal;
using GameProtocal.dto.fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    /// <summary> 战斗数据Model </summary>
    private FightDataModel fightDataModel;

    [SerializeField]
    private Transform[] buildingPos;
    [SerializeField]
    private Transform[] heroStartPos;

    public Dictionary<int, GameObject> teamOneObject = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> teamTwoObject = new Dictionary<int, GameObject>();




    private void Awake()
    {
        fightDataModel = GameDataModelManager.Instance.GetModel<FightDataModel>();
        fightDataModel.RegisterChangeEvent(OnFightModelChange);
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
        }



    }




}
