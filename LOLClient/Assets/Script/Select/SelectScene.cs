using GameProtocal;
using GameProtocal.dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    [SerializeField]
    private GameObject mask;

    [SerializeField]
    private Text chatText;

    [SerializeField]
    private InputField chatInput;

    [SerializeField]
    private ScrollRect chatScrollRect;

    [SerializeField]
    private Button readyButton;

    /// <summary> 英雄列表View </summary>
    [SerializeField]
    private HeroListFrame heroListFrame;

    /// <summary> 数据model </summary>
    private MainDataModel mainDataModel;

    /// <summary> 己方player列表 </summary>
    public PlayerFrame[] leftPlayerFrame;

    /// <summary> 敌方player列表 </summary>
    public PlayerFrame[] rightPlayerFrame;

    /// <summary> 玩家id与PlayerFrame映射 </summary>
    private Dictionary<int, PlayerFrame> playerFrameMap = new Dictionary<int, PlayerFrame>();

    private void Awake()
    {
        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();

        //注册委托
        mainDataModel.RegisterChangeEvent(OnModelValueChange);
        SelectHandler.Instance.OnRoomDestroy += OnRoomDestroy;
        transform.Find("BtnCharSend").GetComponent<Button>().onClick.AddListener(OnBtnSendChatClick);

        readyButton.onClick.AddListener(OnReadyButtonClick);
    }

    private void OnDestroy()
    {
        //注销委托
        mainDataModel.RemoveChangeEvent(OnModelValueChange);
        if (SelectHandler.Instance != null)
        {
            SelectHandler.Instance.OnRoomDestroy -= OnRoomDestroy;
        }
    }

    private void Start()
    {
        //请求进入所在房间
        NetIO.Instance.Write(Protocal.TYPE_SELECT, 0, SelectProtocal.ENTER_CREQ, 0);
    }

    private void OnModelValueChange(AbsGameDataModel.OnValueChangeArgs args)
    {
        switch (args.valueType)
        {
            case (int)MainDataModel.ModelValueType.SelectRoomModelArray:
                OnSelectRoomModelArrayChange((SelectRoomDTO)args.newValue);
                break;

            case (int)MainDataModel.ModelValueType.SelectRoomModel:
                OnSelectRoomModelChange((SelectModel)args.newValue);
                break;

            case (int)MainDataModel.ModelValueType.Chat:
                OnChatChange((List<string>)args.newValue);
                break;
        }
    }

    /// <summary>
    /// 当房间总队友model发生改变时
    /// </summary>
    /// <param name="dto"></param>
    private void OnSelectRoomModelArrayChange(SelectRoomDTO dto)
    {
        //隐藏遮罩
        mask.SetActive(false);

        //刷新UI，刷新显示房间所有队员的UI
        //获取对应的队伍
        List<SelectModel> ourPart = null;
        List<SelectModel> enemyPart = null;
        if (new List<SelectModel>(dto.teamOne).Find(x => x.userId == mainDataModel.UserDTO.id)!=null)
        {
            ourPart = new List<SelectModel>(dto.teamOne);
            enemyPart = new List<SelectModel>(dto.teamTwo);
        }
        if (new List<SelectModel>(dto.teamTwo).Find(x => x.userId == mainDataModel.UserDTO.id) != null)
        {
            ourPart = new List<SelectModel>(dto.teamTwo);
            enemyPart = new List<SelectModel>(dto.teamOne);
        }
        //刷新UI并添加映射
        for(int i = 0; i < ourPart.Count; i++)
        {
            //UI
            leftPlayerFrame[i].RefreshUI(ourPart[i]);
            //添加映射
            playerFrameMap.Add(ourPart[i].userId, leftPlayerFrame[i]);
        }
        for (int i = 0; i < enemyPart.Count; i++)
        {
            //UI
            rightPlayerFrame[i].RefreshUI(enemyPart[i]);
            //添加映射
            playerFrameMap.Add(enemyPart[i].userId, rightPlayerFrame[i]);
        }

        //刷新英雄列表UI
        heroListFrame.RefreshHeroList(mainDataModel.UserDTO.ownHeroList);
    }


    /// <summary>
    /// 当房间某个人的model发生改变时
    /// </summary>
    /// <param name="model"></param>
    private void OnSelectRoomModelChange(SelectModel model)
    {
        //刷新显示该玩家的UI
        if (playerFrameMap.ContainsKey(model.userId))
        {
            playerFrameMap[model.userId].RefreshUI(model);
        }

        //刷新英雄列表UI
        if (!GetOwnModel().isReady)  //准备后不需要刷新英雄列表
        {
            heroListFrame.RefreshHeroList(mainDataModel.UserDTO.ownHeroList);
        }

        //准备按钮及英雄列表
        if (model.userId == mainDataModel.UserDTO.id)
        {
            if (model.hero != -1 && model.isReady==false)
                readyButton.interactable = true;

            if (model.isReady == true)
            {
                //准备后准备按钮不可按
                readyButton.interactable = false;
                //准备后全部英雄变得不可选
                var heroFrames = heroListFrame.GetComponentsInChildren<HeroFrame>();
                foreach(var heroFrame in heroFrames)
                {
                    heroFrame.DeActive();
                }
            }
        }
    }

    private void OnChatChange(List<string> chats)
    {
        //玩家最新发送的聊天文字
        string latestChat = chats[chats.Count - 1];

        //添加显示到UI
        chatText.text += latestChat + "\n";

        chatScrollRect.verticalNormalizedPosition = 0;
    }

    private void OnRoomDestroy()
    {
        //清空房间数据
        mainDataModel.CleanRoomData();
        //返回主场景
        SceneManager.LoadScene("main");
    }

    private void OnBtnSendChatClick()
    {
        //发送聊天信息
        NetIO.Instance.Write(Protocal.TYPE_SELECT, 0, SelectProtocal.TALK_CREQ, chatInput.text);
        chatInput.text = "";
    }

    /// <summary>
    /// 准备按钮按下
    /// </summary>
    private void OnReadyButtonClick()
    {
        //准备请求
        NetIO.Instance.Write(Protocal.TYPE_SELECT, 0, SelectProtocal.READY_CREQ, 0);
    }

    private SelectModel GetOwnModel()
    {
        //获取对应的队伍
        List<SelectModel> ourPart = null;
        SelectRoomDTO dto = mainDataModel.SelectRoomDTO;
        if (new List<SelectModel>(dto.teamOne).Find(x => x.userId == mainDataModel.UserDTO.id) != null)
        {
            ourPart = new List<SelectModel>(dto.teamOne);
        }
        if (new List<SelectModel>(dto.teamTwo).Find(x => x.userId == mainDataModel.UserDTO.id) != null)
        {
            ourPart = new List<SelectModel>(dto.teamTwo);
        }
        foreach(var model in ourPart)
        {
            if (model.userId == mainDataModel.UserDTO.id)
            {
                return model;
            }
        }

        return null;
    }

}
