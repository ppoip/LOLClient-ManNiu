using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsGameDataModel 
{
    private Action<OnValueChangeArgs> OnValueChangeEvent;

    public void RegisterChangeEvent(Action<OnValueChangeArgs> args)
    {
        OnValueChangeEvent += args;
    }

    public void RemoveChangeEvent(Action<OnValueChangeArgs> args)
    {
        OnValueChangeEvent -= args;
    }

    protected void BroadcastEvent(OnValueChangeArgs args)
    {
        if (OnValueChangeEvent != null)
        {
            OnValueChangeEvent(args);
        }
    }

    
    public class OnValueChangeArgs
    {
        public int valueType;
        public object oldValue;
        public object newValue;
    }
}
