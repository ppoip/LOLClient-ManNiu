using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using GameCommon;

public class NetIO
{
    private static readonly object _lock = new object();
    private static NetIO _instance;
    public static NetIO Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new NetIO();
                }
                return _instance;
            }
        }
    }

    private Socket socket;
    private string ip= "127.0.0.1";
    private int port=6550;

    /// <summary> receive 临时缓冲区 </summary>
    private byte[] receiveBuffer = new byte[1024];

    /// <summary> receive 处理缓存 </summary>
    private List<byte> cache = new List<byte>();

    /// <summary> 是否正在读cache </summary>
    private bool isReading = false;

    /// <summary> 接收到的全部SocketModel </summary>
    public List<SocketModel> messages = new List<SocketModel>();

    private NetIO()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, receiveBuffer);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            //结束接收
            int length = socket.EndReceive(ar);
            //接收到的数据
            byte[] data = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, data, 0, length);
            //加入到缓存
            cache.AddRange(data);

            if (!isReading)
            {
                isReading = true;
                //处理缓存中的数据
                OnData();
            }

            //递归
            socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, receiveBuffer);
        }
        catch (Exception e)
        {
            Debug.LogError("服务器主动断开连接");
            socket.Close();
        }

    }

    /// <summary>
    /// cache中有数据
    /// </summary>
    void OnData()
    {
        byte[] data = null;

        //长度解码
        data = LengthEncoding.decode(ref cache);
        if (data == null)
        {
            //消息未接收完全，退出
            isReading = false;
            return;
        }

        //反序列化data
        object message = MessageEncoding.Decode(data);

        //消息处理
        messages.Add(message as SocketModel);

        //递归
        OnData();
    }

    /// <summary>
    /// 想服务器发送数据模型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public void Write(byte type, int area, int command, object message)
    {
        SocketModel sm = new SocketModel(type, area, command, message);
        //message encode
        byte[] data = MessageEncoding.Encode(sm);
        //lenght encode
        data = LengthEncoding.encode(data);
        try
        {
            //send
            socket.Send(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
