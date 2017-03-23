using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataModelManager : MonoSingleton<GameDataModelManager>
{
    private Dictionary<string, AbsGameDataModel> models = new Dictionary<string, AbsGameDataModel>();

    public T GetModel<T>() where T : AbsGameDataModel
    {
        if (models.ContainsKey(typeof(T).Name))
        {
            return (T)models[typeof(T).Name];
        }
        
        //实例化
        AbsGameDataModel model = Activator.CreateInstance<T>();
        models.Add(typeof(T).Name, model);
        return (T)model;
    }
}
