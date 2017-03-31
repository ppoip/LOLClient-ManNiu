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

    MainDataModel mainDataModel;

    private void Awake()
    {
        mainDataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();

        //注册委托
        mainDataModel.RegisterChangeEvent(OnModelValueChange);
        SelectHandler.Instance.OnRoomDestroy += OnRoomDestroy;
        transform.Find("BtnCharSend").GetComponent<Button>().onClick.AddListener(OnBtnSendChatClick);
    }

    private void OnDestroy()
    {
        //注销委托
        mainDataModel.RegisterChangeEvent(OnModelValueChange);
        SelectHandler.Instance.OnRoomDestroy -= OnRoomDestroy;
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
        //TODO

        //显示玩家拥有的英雄列表
        //TODO
    }

    /// <summary>
    /// 当房间某个人的model发生改变时
    /// </summary>
    /// <param name="model"></param>
    private void OnSelectRoomModelChange(SelectModel model)
    {
        //刷新显示该玩家的UI
        //TODO
    }

    private void OnChatChange(List<string> chats)
    {
        //玩家最新发送的聊天文字
        string latestChat = chats[chats.Count - 1];

        //添加显示到UI
        chatText.text += latestChat + "\n";

        var pos = chatText.GetComponent<RectTransform>().position;
        chatText.GetComponent<RectTransform>().position = new Vector3(pos.x, chatText.GetComponent<RectTransform>().rect.height, pos.z);
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

}
