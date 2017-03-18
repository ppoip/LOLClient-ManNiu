using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocal;
using GameProtocal.dto;

public class LoginPanel : MonoBehaviour
{
    public InputField inputAccount;
    public InputField inputPassword;

    private void Awake()
    {

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

}
