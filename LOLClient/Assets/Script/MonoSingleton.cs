using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (MonoSingleton<T>._instance == null)
            {
                MonoSingleton<T>._instance = FindObjectOfType<T>();
            }
            return MonoSingleton<T>._instance;
        }
    }
}
