using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    private void Awake()
    {
        var a = NetIO.Instance;
    } 

    public void OnBtnLoginClick()
    {
        Debug.Log("login...");
    }

}
