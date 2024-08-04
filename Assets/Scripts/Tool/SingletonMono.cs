using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> //ע���Լ��ΪT����Ϊ�䱾�������
{
    private static T instance; //����˽�ж����¼ȡֵ����ֻ��ֵһ�α����θ�ֵ

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




