using GameProtocal;
using GameProtocal.dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private CreateRolePanel createRolePanel;

    [SerializeField]
    private GameObject touchMask;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Slider expSlider;

    [SerializeField]
    private Text matchText;

    [SerializeField]
    private Button matchButton;


    /// <summary> 数据Model </summary>
    private MainDataModel dataModel;

    private void Awake()
    {
        //请求角色信息
        NetIO.Instance.Write(Protocal.TYPE_USER, 0, UserProtocal.GET_INFO_CREQ,0);

        //注册弹出创建召唤师窗口
        UserHandler.Instance.OnShowCreatePanel += ShowCreateUserPanel;
        UserHandler.Instance.OnHideCreatePanel += HideCreateUserPanel;

        //注册隐藏Mask
        UserHandler.Instance.OnHideMask += HideTouchMask;

        //获取MainModel
        dataModel = GameDataModelManager.Instance.GetModel<MainDataModel>();

        //注册Model委托
        dataModel.RegisterChangeEvent(OnModelValueChange);

        //Click
        matchButton.onClick.AddListener(OnMatchButtonClick);
    }

    private void OnDestroy()
    {
        if (UserHandler.Instance != null) //UserHandler可能比自己先释放
        {
            //注销弹出创建召唤师窗口
            UserHandler.Instance.OnShowCreatePanel -= ShowCreateUserPanel;
            UserHandler.Instance.OnHideCreatePanel -= HideCreateUserPanel;

            //注销隐藏Mask
            UserHandler.Instance.OnHideMask -= HideTouchMask;
        }

        //注销Model委托
        dataModel.RemoveChangeEvent(OnModelValueChange);
    }

    /// <summary>
    /// 显示创建召唤师窗口
    /// </summary>
    private void ShowCreateUserPanel()
    {
        createRolePanel.Show();
    }

    /// <summary>
    /// 隐藏创建召唤师窗口
    /// </summary>
    private void HideCreateUserPanel()
    {
        createRolePanel.Hide();
    }

    /// <summary>
    /// 隐藏Mask
    /// </summary>
    private void HideTouchMask()
    {
        touchMask.SetActive(false);
    }

    /// <summary>
    /// 当MainModel有属性改变时
    /// </summary>
    /// <param name="args"></param>
    private void OnModelValueChange(AbsGameDataModel.OnValueChangeArgs args)
    {
        switch (args.valueType)
        {
            case (int)MainDataModel.ModelValueType.UserDTO:
                OnUserDTOChange(args.oldValue as UserDTO, args.newValue as UserDTO);
                break;
        }
    }

    private void OnUserDTOChange(UserDTO oldValue,UserDTO newValue)
    {
        //显示名称
        nameText.text = newValue.name + " 等级:"+newValue.level;

        //ExpBar
        expSlider.value = newValue.exp / 100.0f;
    }

    private void OnMatchButtonClick()
    {
        if (matchText.text == "开始排队")
        {
            matchText.text = "退出排队";
            //发送排队请求
            NetIO.Instance.Write(Protocal.TYPE_MATCH, 0, MatchProtocal.ENTER_CREQ, 0);
        }
        else
        {
            matchText.text = "开始排队";
        }
    }
}
