using GameCommon;
using GameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMessageUtil : MonoBehaviour
{
    /// <summary> 游戏里所有handler </summary>
    public Dictionary<byte, IHandler> handlers = new Dictionary<byte, IHandler>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    } 

    private void Update()
    {
        while (NetIO.Instance.messages.Count > 0)
        {
            SocketModel sm = NetIO.Instance.messages[0];
            NetIO.Instance.messages.RemoveAt(0);
            MessageProcess(sm);
        }
    }

    /// <summary>
    /// 处理一个SocketModel
    /// </summary>
    /// <param name="model"></param>
    private void MessageProcess(SocketModel model)
    {
        if (handlers.ContainsKey(model.type))
        {
            //找到对应的handler
            handlers[model.type].OnMessageReceived(model);
        }
    }
}
