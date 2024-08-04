using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> //注意此约束为T必须为其本身或子类
{
    private static T instance; //创建私有对象记录取值，可只赋值一次避免多次赋值

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
            }
            return instance;

        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
    }

}




