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
        AccountInfoDTO accountDTO = new AccountInfoDTO() { account=inputAccount.text, password=inputPassword.text };
        NetIO.Instance.Write(Protocal.TYPE_LOGIN, 0, LoginProtocal.LOGIN_CREQ, accountDTO);
    }

}
