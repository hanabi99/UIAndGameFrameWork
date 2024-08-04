using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttrType : uint
{
    // �±�
    Recruit,
    // ��ʿ
    StaffSergeant,
    // ��ʿ
    Sergeant,
    // ��ξ
    Captian,
}

public class FlyweightMain : MonoBehaviour
{
    public static readonly Dictionary<AttrType, Dictionary<string, object>> BaseAttrDict = new Dictionary<AttrType, Dictionary<string, object>>
 {
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",100},
         {"MoveSpeed",1.0f},
         {"Name","�±�"}
     }},
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",200},
         {"MoveSpeed",2.0f},
         {"Name","��ʿ"}
     }},
     {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",500},
         {"MoveSpeed",3.0f},
         {"Name","��ʿ"}
     }},  {AttrType.Recruit , new Dictionary<string, object>
     {
         {"MaxHp",1000},
         {"MoveSpeed",4.0f},
         {"Name","��ξ"}
     }},
 };


    void Start()
    {
        AttrFactory factory = new AttrFactory();
        for (int i = 0; i < 100; i++)
        {
            var values = Enum.GetValues(typeof(AttrFactory.AttrType));
            AttrFactory.AttrType attrType = (AttrFactory.AttrType)values.GetValue(UnityEngine.Random.Range(0, 3));
            SoldierAttr soldierAttr = factory.GetSoldierAttr(attrType, UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(155.0f, 190.0f));
        }
    }




}
public class SoldierAttr
{
    public int hp { get; set; }

    public float height { get; set; }
    public FlyweightAttr flyweightAttr { get; }

    // ���캯��
    public SoldierAttr(FlyweightAttr flyweightAttr, int hp, float height)
    {
        this.flyweightAttr = flyweightAttr;
        this.hp = hp;
        this.height = height;
    }

    public int GetMaxHp()
    {
        return flyweightAttr.maxHp;
    }


    public float GetMoveSpeed()
    {
        return flyweightAttr.moveSpeed;
    }


    public string GetName()
    {
        return flyweightAttr.name;
    }
}


public class FlyweightAttr
{
    public int maxHp { get; set; }
    public float moveSpeed { get; set; }
    public string name { get; set; }
    public FlyweightAttr(string name, int maxHp, float moveSpeed)
    {
        this.name = name;
        this.maxHp = maxHp;
        this.moveSpeed = moveSpeed;
    }
}
public class AttrFactory
{
    /// <summary>
    /// ��������ö��
    /// </summary>
    public enum AttrType : uint
    {
        // �±�
        Recruit = 0,
        // ��ʿ
        StaffSergeant,
        // ��ʿ
        Sergeant,
        // ��ξ
        Captian,
    }
    /// <summary>
    /// �������Ի���
    /// </summary>
    private Dictionary<AttrType, FlyweightAttr> _flyweightAttrDB = null;
    public AttrFactory()
    {
        _flyweightAttrDB = new Dictionary<AttrType, FlyweightAttr>();
        _flyweightAttrDB.Add(AttrType.Recruit, new FlyweightAttr("ʿ��", 100, 1.0f));
        _flyweightAttrDB.Add(AttrType.StaffSergeant, new FlyweightAttr("��ʿ", 200, 2.0f));
        _flyweightAttrDB.Add(AttrType.Sergeant, new FlyweightAttr("��ʿ", 500, 3.0f));
        _flyweightAttrDB.Add(AttrType.Captian, new FlyweightAttr("��ξ", 1000, 4.0f));
    }
    /// <summary>
    /// ��ȡ��ɫ����
    /// </summary>
    /// <param name="type">����</param>
    /// <param name="hp">Ѫ��</param>
    /// <param name="height">���</param>
    /// <returns></returns>
    public SoldierAttr GetSoldierAttr(AttrType type, int hp, float height)
    {
        if (!_flyweightAttrDB.ContainsKey(type))
        {
            Debug.LogErrorFormat("{0}���Բ�����", type);
            return null;
        }
        FlyweightAttr flyweightAttr = _flyweightAttrDB[type];
        SoldierAttr attr = new SoldierAttr(flyweightAttr, hp, height);
        return attr;
    }
}


