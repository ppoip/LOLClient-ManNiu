using GameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRolePanel : MonoBehaviour
{
    public Button btnCreate;
    public InputField inputRoleName;

    private void Awake()
    {
        btnCreate.onClick.AddListener(OnBtnCreateClick);
    }

	public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBtnCreateClick()
    {
        //请求创建角色
        NetIO.Instance.Write(Protocal.TYPE_USER, 0, UserProtocal.CREATE_CREQ, inputRoleName.text);
    }
}
