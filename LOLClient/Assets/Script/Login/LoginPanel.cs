using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocal;
using GameProtocal.dto;

public class LoginPanel : MonoBehaviour
{
    private static LoginPanel _instance;
    public static LoginPanel Instance { get{ return _instance; } }

    public InputField inputAccount;
    public InputField inputPassword;
    public Text messageText;

    private void Awake()
    {
        _instance = this;
    } 

    public void OnBtnLoginClick()
    {
        //account数据包
        AccountInfoDTO accountDTO = new AccountInfoDTO() { account=inputAccount.text, password=inputPassword.text };
        //发包
        NetIO.Instance.Write(Protocal.TYPE_LOGIN, 0, LoginProtocal.LOGIN_CREQ, accountDTO);
    }

    public void OnBtnRegisterClick()
    {
        //account数据包
        AccountInfoDTO accountDTO = new AccountInfoDTO() { account = inputAccount.text, password = inputPassword.text };
        //发包
        NetIO.Instance.Write(Protocal.TYPE_LOGIN, 0, LoginProtocal.REG_CREQ, accountDTO);
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }
}
