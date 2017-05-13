using GameProtocal;
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

    [SerializeField]
    private GameObject damageNumParent;

    public Dictionary<int, GameObject> teamOneObject = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> teamTwoObject = new Dictionary<int, GameObject>();

    /// <summary> 我方所在队伍 </summary>
    public int teamCode = -1;

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

            case (int)FightDataModel.ModelValueType.HeroMove:
                OnHeroMoveEvent(args.newValue as HeroMoveDto);
                break;

            case (int)FightDataModel.ModelValueType.HeroAttack:
                OnHeroAttackEvent(args.newValue as HeroAttackDto);
                break;

            case (int)FightDataModel.ModelValueType.Damage:
                OnDamageEvent(args.newValue as DamageDTO);
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
                //标记自身英雄
                selfHero = go;
                //标记所属队伍
                teamCode = 1;
                //刷新所有UI
                RefreshAllUI(item as PlayerFightModel);
            }

            if (item.modelType == ModelType.Human)
            {
                //设置英雄数据
                go.GetComponent<PlayerController>().Data = item as PlayerFightModel;
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
                //标记自身英雄
                selfHero = go;
                //标记所属队伍
                teamCode = 2;
                //刷新所有UI
                RefreshAllUI(item as PlayerFightModel);
            }

            if (item.modelType == ModelType.Human)
            {
                //设置英雄数据
                go.GetComponent<PlayerController>().Data = item as PlayerFightModel;
            }
        }

        //相机看向自己的英雄
        mainCamera.LookAtTarget(selfHero.transform, new Vector3(-43.21f, -66.08f, -165.35f));

        //初始化战斗对象的层Layer
        InitObjLayer();
    }

    /// <summary>
    /// 初始化战斗对象的层
    /// </summary>
    private void InitObjLayer()
    {
        List<GameObject> ourparty = GetOurPartyObjs();
        List<GameObject> enemyparty = GetEnemyPartyObjs();

        foreach (var item in ourparty)
        {
            item.gameObject.layer = 10; //OurParty Layer
            //Transform[] children = item.GetComponentsInChildren<Transform>();
            //foreach (var child in children)
            //{
            //    child.gameObject.layer = 10;
            //}
        }

        foreach (var item in enemyparty)
        {
            item.gameObject.layer = 11; //EnemyParty Layer
            //Transform[] children = item.GetComponentsInChildren<Transform>();
            //foreach (var child in children)
            //{
            //    child.gameObject.layer = 11;
            //}
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

    }





    /// <summary>
    /// 当鼠标右键点击地面
    /// </summary>
    /// <param name="pos"></param>
    public void OnMapGroundRightClick(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        var hits = Physics.RaycastAll(ray, 220);
        for (int i = 0; i < hits.Length; i++) 
        {
            RaycastHit item = hits[i];

            //如果是敌人则普通攻击
            if(item.transform.gameObject.layer == 11/*LayerMask.GetMask("EnemyParty")*/)
            {
                PlayerController enemyCtrl = item.transform.GetComponent<PlayerController>();
                float distance = Vector3.Distance(selfHero.transform.position, item.transform.position);
                if (distance <= selfHero.GetComponent<PlayerController>().Data.atkRange)
                {
                    //敌人在攻击范围内
                    //发送普通攻击请求给服务器
                    NetIO.Instance.Write(Protocal.TYPE_FIGHT, 0, FightProtocal.ATTACK_CREQ, enemyCtrl.Data.id);
                    break;
                }
                else
                {
                    //敌人不在攻击范围内
                    continue;
                }
            }

            //如果是地板则寻路
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("MapGround"))
            {
                HeroMoveDto dto = new HeroMoveDto()
                {
                     x = item.point.x,
                     y = item.point.y,
                     z = item.point.z
                };
                
                //发送移动请求
                FightHandler.Instance.SendMoveRequest(dto);
            }
        }
    }

    /// <summary>
    /// 当地图有英雄申请移动
    /// </summary>
    /// <param name="dto"></param>
    private void OnHeroMoveEvent(HeroMoveDto dto)
    {
        GameObject targetHero = null;
        if (teamOneObject.ContainsKey(dto.userId))
        {
            targetHero = teamOneObject[dto.userId];
        }
        if (teamTwoObject.ContainsKey(dto.userId))
        {
            targetHero = teamTwoObject[dto.userId];
        }

        targetHero.SendMessage("MoveTo", new Vector3() { x = dto.x, y = dto.y, z = dto.z });
    }

    /// <summary>
    /// 当地图有英雄申请普通
    /// </summary>
    /// <param name="dto"></param>
    private void OnHeroAttackEvent(HeroAttackDto dto)
    {
        //Src
        PlayerController ctrl = null;
        if (teamOneObject.ContainsKey(dto.srcId))
        {
            ctrl = teamOneObject[dto.srcId].GetComponent<PlayerController>();
        }
        if (teamTwoObject.ContainsKey(dto.srcId))
        {
            ctrl = teamTwoObject[dto.srcId].GetComponent<PlayerController>();
        }

        //Target
        GameObject targetObj = null;
        if (teamOneObject.ContainsKey(dto.targetId))
        {
            targetObj = teamOneObject[dto.targetId];
        }
        if (teamTwoObject.ContainsKey(dto.targetId))
        {
            targetObj = teamTwoObject[dto.targetId];
        }

        ctrl.Attack(new GameObject[] { targetObj });
    }

    /// <summary>
    /// 当有英雄建筑小兵收到伤害时
    /// </summary>
    /// <param name="dto"></param>
    private void OnDamageEvent(DamageDTO dto)
    {
        foreach (var item in dto.parameters)
        {
            //被攻击对象的控制类
            PlayerController playerCtrl = GetTeamObjectById(item[0]).GetComponent<PlayerController>();
            playerCtrl.Data.curHp -= item[1];

            //显示伤害数字
            GameObject numTextPrefab = Resources.Load<GameObject>("UIPrefabs/DamageNum");
            GameObject numText = GameObject.Instantiate(numTextPrefab, damageNumParent.transform);
            numText.GetComponent<DamageNum>().SetText(item[1].ToString());
            numText.transform.localScale = Vector3.one;
            (numText.transform as RectTransform).position = Camera.main.WorldToScreenPoint(GetTeamObjectById(item[0]).transform.position) + Vector3.up * 25;


            //刷新头顶血条
            playerCtrl.RefreshUI();

            if (item[0] >= 0)  //英雄 
            {
                if (item[0] == mainDataModel.UserDTO.id)
                {
                    //被攻击的是自己的英雄，刷新UI血量
                    RefreshAllUI(playerCtrl.Data);
                }

                if (item[2] == 0)
                {
                    //英雄死亡，暂时隐藏
                    GetTeamObjectById(item[0]).SetActive(false);
                }
            }
            else  //小兵以及建筑
            {
                if (item[2] == 0)
                {
                    //死亡 销毁游戏对象
                    Destroy(GetTeamObjectById(item[0]));
                }
            }

        }
    }

    /// <summary>
    /// 获取我方所有对象
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetOurPartyObjs()
    {
        List<GameObject> objs = new List<GameObject>();
        if (teamCode == 1)
        {
            foreach (var item in teamOneObject.Values)
            {
                objs.Add(item);
            }
        }
        else
        {
            foreach (var item in teamTwoObject.Values)
            {
                objs.Add(item);
            }
        }
        return objs;
    }

    /// <summary>
    /// 获取敌方所有对象
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetEnemyPartyObjs()
    {
        List<GameObject> objs = new List<GameObject>();
        if (teamCode == 1)
        {
            foreach (var item in teamTwoObject.Values)
            {
                objs.Add(item);
            }
        }
        else
        {
            foreach (var item in teamOneObject.Values)
            {
                objs.Add(item);
            }
        }
        return objs;
    }

    private GameObject GetTeamObjectById(int id)
    {
        if (teamOneObject.ContainsKey(id))
            return teamOneObject[id];

        return teamTwoObject[id];
    }

}
